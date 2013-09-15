using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBot.CampfireAdapter
{
    public class CampfireMessage
    {
        public string id { get; set; }
        public string type { get; set; }
        public string user_id { get; set; }
        public string room_id { set; get; }
        public string body { get; set; }
        public DateTime created_at { get; set; }
        public bool starred { get; set; }
    }
}
