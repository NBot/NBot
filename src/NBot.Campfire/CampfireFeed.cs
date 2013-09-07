using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NBot.Core;

namespace NBot.Campfire
{
    public class CampfireFeed : IMessageFeed
    {
        private readonly string _authorizationHeader;
        private readonly IComponentContext _container;
        readonly List<CampfireRoomFeed> _roomFeeds = new List<CampfireRoomFeed>();
        private readonly List<int> _roomsToJoin;

        public CampfireFeed(IComponentContext container)
        {
            _container = container;

            var token = Core.NBot.Settings["Token"];
            string auth = string.Format("{0}:X", token);

            _authorizationHeader = string.Format("Basic {0}", Convert.ToBase64String(Encoding.ASCII.GetBytes(auth)));

            _roomsToJoin = Core.NBot.Settings["RoomsToJoin"] as List<int>;
        }

        #region IMessageFeed Members

        public void StartFeed()
        {
            foreach (int roomId in _roomsToJoin)
            {
                Listen(roomId);
            }
        }

        public void StopFeed()
        {
            foreach (CampfireRoomFeed campfireRoomListener in _roomFeeds)
            {
                campfireRoomListener.StopListening();
            }
        }

        #endregion

        private void Listen(int roomId)
        {
            var roomListener = new CampfireRoomFeed(roomId, _container.Resolve<Account>(), _authorizationHeader, _container);
            _roomFeeds.Add(roomListener);
            roomListener.StartListening();
        }
    }
}