using System;
using System.Threading.Tasks;

namespace NBot.ConsoleAdapter
{
    public class ConsoleListner
    {
        private readonly Action<string> _messageRecieved;
        private bool _isListening = true;

        public ConsoleListner(Action<string> messageRecieved)
        {
            _messageRecieved = messageRecieved;
        }

        public void StartListening()
        {
            Task.Factory.StartNew(() =>
            {
                while (_isListening)
                {
                    string line = Console.ReadLine();

                    if (line == null)
                    {
                        return;
                    }

                    Task.Factory.StartNew(() => _messageRecieved(line));
                }
            });
        }

        public void StopListening()
        {
            _isListening = false;
        }
    }
}