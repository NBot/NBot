using System;
using System.Collections.Generic;
using NBot.Core;

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
            foreach (var roomId in _roomsToJoin)
            {
                var listener = new CampfireRoomListener(_token, _account, roomId, OnMessageProduced);
                _listeners.Add(listener);
                listener.StartListening();
            }
        }

        public void StopProduction()
        {
            foreach (var campfireRoomListener in _listeners)
            {
                campfireRoomListener.StopListening();
            }
        }

        protected virtual void OnMessageProduced(Message message)
        {
            MessageProducedHandler handler = MessageProduced;
            if (handler != null) handler(message);
        }
    }
}