using NBot.ConsoleAdapter;
using NBot.Core;

namespace NBot.Adapters
{
    public class ConsoleAdapter : IAdapter
    {

        public ConsoleAdapter()
        {

            Producer = new ConsoleMessageProducer();
            Client = new ConsoleMessageClient();
        }

        public IMessageProducer Producer { get; set; }
        public IMessageClient Client { get; set; }
    }
}