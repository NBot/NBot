using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBot.Core;
using ServiceStack.Common;
using ServiceStack.Common.Extensions;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;

namespace NBot.CampfireAdapter
{
    public class CampfireMessageProducer : IMessageProducer
    {
        private readonly string _token;
        private readonly string _account;
        private readonly List<int> _roomsToJoin;
        private readonly List<CampfireRoomListener> _listeners = new List<CampfireRoomListener>();

        public CampfireMessageProducer(string token, string account, List<int> roomsToJoin)
        {
            _token = token;
            _account = account;
            _roomsToJoin = roomsToJoin;
        }

        public string Channel { get; private set; }

        public event MessageProducedHandler MessageProduced;

        public void StarProduction()
        {
            var client = new JsonServiceClient("https://{0}.campfirenow.com".FormatWith(_account)) { UserName = _token, Password = "X" };
            var user = client.Get<CampfireUserWrapper>("/users/me.json").User;

            Parallel.ForEach(_roomsToJoin, (roomId) =>
            {
                client.Post<string>("/room/{0}/join.json".FormatWith(roomId), string.Empty);
                var roomListenerContext = new CampfireRoomListenerContext(roomId, _token, user.Id.ToInt(), OnMessageProduced);
                var roomListener = new CampfireRoomListener(roomListenerContext);
                _listeners.Add(roomListener);
                roomListener.StartListening();
            });
        }

        public void StopProduction()
        {
            var client = new JsonServiceClient("https://{0}.campfirenow.com".FormatWith(_account)) { UserName = _token, Password = "X" };
            
            Parallel.ForEach(_listeners, (campfireRoomListener) =>
            {
                client.Post<string>("/room/{0}/leave.json".FormatWith(campfireRoomListener.Context.RoomId), string.Empty);
                campfireRoomListener.StopListening();
            });
        }

        protected virtual void OnMessageProduced(Message message)
        {
            MessageProducedHandler handler = MessageProduced;
            if (handler != null) handler(message);
        }
    }
}