using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NBot.Core;
using ServiceStack.Common.Extensions;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;

namespace NBot.CampfireAdapter
{
    public class CampfireRoomListener
    {
        private readonly int _roomId;
        private readonly Action<Message> _messageRecieved;
        private bool _isListening;
        private readonly HttpWebRequest _request;
        private readonly JsonServiceClient _client;
        private CampfireUser _user;


        public CampfireRoomListener(string token, string account, int roomId, Action<Message> messageRecieved)
        {
            string auth = string.Format("{0}:X", token);
            string authorizationHeader = string.Format("Basic {0}", Convert.ToBase64String(Encoding.ASCII.GetBytes(auth)));
            _roomId = roomId;
            _messageRecieved = messageRecieved;
            _request = (HttpWebRequest)WebRequest.Create(string.Format("https://streaming.campfirenow.com/room/{0}/live.json", roomId));
            _client = new JsonServiceClient("https://{0}.campfirenow.com".FormatWith(account)) { UserName = token, Password = "X" };
            _request.Headers.Add(HttpRequestHeader.Authorization, authorizationHeader);
        }

        public void StartListening()
        {
            _isListening = true;
            _user = _client.Get<CampfireUserWrapper>("/users/me.json").User;

            Task.Factory.StartNew(() =>
            {
                try
                {
  
                    _client.Post<string>("/room/{0}/join.json".FormatWith(_roomId), string.Empty);
                    WebResponse response = _request.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            while (stream.CanRead && _isListening)
                            {
                                int value = stream.ReadByte();

                                if (value == -1)
                                    break;
                                if (value == 32)
                                    continue;


                                // message is hear time to read.....
                                string message = ((char)value).ToString();

                                do
                                {
                                    value = stream.ReadByte();

                                    if (value == -1)
                                        break;
                                    message += (char)value;
                                } while (value != 13);

                                // Run on another process and keep listening :)
                                Task.Factory.StartNew(() => OnMessageRecieved(message));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    StopListening();
                    StartListening();
                }
            });
        }

        private void OnMessageRecieved(string message)
        {
            var campfireMessage = message.FromJson<CampfireMessage>();

            if (campfireMessage.user_id == _user.Id)
                return;

            _messageRecieved(BuildMessage(campfireMessage));
        }

        public void StopListening()
        {
            _isListening = false;
            // Leave Room
            _client.Post<string>("/room/{0}/leave.json".FormatWith(_roomId), string.Empty);
        }

        private Message BuildMessage(CampfireMessage message)
        {
            return new Message
            {
                Channel = "Campfire",
                RoomId = message.room_id,
                UserId = message.user_id,
                Content = message.body,
                Type = message.type
            };
        }
    }
}
