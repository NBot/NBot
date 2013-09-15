using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBot.Core;

namespace NBot.CampfireAdapter
{
    public class CampfireAdapter : IAdapter
    {
        public CampfireAdapter(string token, string account, List<int> roomsToJoin)
        {
            Producer = new CampfireMessageProducer(token, account, roomsToJoin);
            Client = new CampfireMessageClient(token, account);
        }

        public IMessageProducer Producer { get; private set; }
        public IMessageClient Client { get; private set; }
    }
}
