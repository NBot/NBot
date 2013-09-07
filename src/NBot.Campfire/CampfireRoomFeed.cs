using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using NBot.Campfire.Messages.IncomingMessages;
using NBot.Core.Brains;
using NBot.Core.Extensions;
using NBot.Core.Messaging;

namespace NBot.Campfire
{
    public class CampfireRoomFeed
    {
        private readonly Account _account;
        private readonly IBrain _brain;
        private readonly IMessagingService _messagingService;
        private readonly HttpWebRequest _request;
        private readonly Room _room;
        private readonly User _user;
        private bool _isListening = true;

        public CampfireRoomFeed(int roomId, Account account, string authorizationHeader, IComponentContext container)
        {
            _account = account;
            _room = account.GetRoom(roomId);
            _user = container.Resolve<User>();
            _messagingService = container.Resolve<IMessagingService>();
            _brain = container.Resolve<IBrain>();
            _request = WebRequest.CreateHttp(string.Format("https://streaming.campfirenow.com/room/{0}/live.json", roomId));
            _request.Headers.Add(HttpRequestHeader.Authorization, authorizationHeader);
        }

        public void StartListening()
        {
            Task.Factory.StartNew(() =>
                                      {
                                          try
                                          {
                                              // I did it this way for speed
                                              _room.Join();
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
                                                          string message = ((char) value).ToString();

                                                          do
                                                          {
                                                              value = stream.ReadByte();

                                                              if (value == -1)
                                                                  break;
                                                              message += (char) value;
                                                          } while (value != 13);
                                                          // Run on another process and keep listening :)
                                                          Task.Run(() => OnJsonMessageRecieved(message));
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

        public void StopListening()
        {
            _isListening = false;
            _room.Leave();
        }

        protected virtual void OnJsonMessageRecieved(string json)
        {
            var message = json.FromJson<RoomMessage>();

            switch (message.Type)
            {
                case "TextMessage":
                    message = json.FromJson<TextMessage>();
                    break;
                case "PasteMessage":
                    message = json.FromJson<PasteMessage>();
                    break;
                case "SoundMessage":
                    message = json.FromJson<SoundMessage>();
                    break;
                case "AdvertisementMessage":
                    message = json.FromJson<AdvertisementMessage>();
                    break;
                case "AllowGuestsMessage":
                    message = json.FromJson<AllowGuestsMessage>();
                    break;
                case "DisallowGuestsMessage":
                    message = json.FromJson<DisallowGuestsMessage>();
                    break;
                case "IdleMessage":
                    message = json.FromJson<IdleMessage>();
                    break;
                case "KickMessage":
                    message = json.FromJson<KickMessage>();
                    break;
                case "LeaveMessage":
                    message = json.FromJson<LeaveMessage>();
                    break;
                case "EnterMessage":
                    message = json.FromJson<EnterMessage>();
                    break;
                case "SystemMessage":
                    message = json.FromJson<SystemMessage>();
                    break;
                case "TimestampMessage":
                    message = json.FromJson<TimestampMessage>();
                    break;
                case "TopicChangeMessage":
                    message = json.FromJson<TopicChangeMessage>();
                    break;
                case "UnidleMessage":
                    message = json.FromJson<UnidleMessage>();
                    break;
                case "LockMessage":
                    message = json.FromJson<LockMessage>();
                    break;
                case "UnlockMessage":
                    message = json.FromJson<UnlockMessage>();
                    break;
                case "UploadMessage":
                    message = json.FromJson<UploadMessage>();
                    break;
                case "ConferenceCreatedMessage":
                    message = json.FromJson<ConferenceCreatedMessage>();
                    break;
                case "ConferenceFinishedMessage":
                    message = json.FromJson<ConferenceFinishedMessage>();
                    break;
            }

            // Dont send messages that I posted
            var userMessage = message as UserMessage;
            if (userMessage != null)
            {
                if (!_brain.ContainsKey(GetUserKey(userMessage.UserId)))
                {
                    _brain.SetValue(GetUserKey(userMessage.UserId), _account.GetUser(userMessage.UserId));
                }

                if (userMessage.UserId == _user.Id)
                {
                    return;
                }
            }

            _messagingService.Publish(message);
        }

        private string GetUserKey(int userId)
        {
            return string.Format("User_{0}", userId);
        }
    }
}