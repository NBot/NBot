using System;
using NBot.Core;

namespace NBot.ConsoleAdapter
{
    public class ConsoleMessageProducer : IMessageProducer
    {
        private ConsoleListner _listner;

        public event MessageProducedHandler MessageProduced;

        public string Channel
        {
            get { return "ConsoleChannel"; }
        }

        public void StarProduction()
        {
            Console.Clear();
            Action<string> messageRcieved = line =>
            {
                var message = new Message
                {
                    Channel = Channel,
                    Type = "TestMessage",
                    RoomId = "ConsoleRoom",
                    UserId = "ConsoleUser",
                    Content = line
                };

                FireMessageProduced(message);
            };

            _listner = new ConsoleListner(messageRcieved);

            _listner.StartListening();
        }

        public void StopProduction()
        {
            _listner.StopListening();
        }

        protected virtual void FireMessageProduced(Message message)
        {
            MessageProducedHandler handler = MessageProduced;
            if (handler != null) handler(message);
        }
    }
}