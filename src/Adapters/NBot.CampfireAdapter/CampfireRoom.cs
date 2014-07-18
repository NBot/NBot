using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBot.Core;

namespace NBot.CampfireAdapter
{
    public class CampfireRoom : IEntity
    {
        public bool locked { get; set; }
        public DateTime created_at { get; set; }
        public string topic { get; set; }
        public string Id { get; set; }
        public int membership_limit { get; set; }
        public DateTime updated_at { get; set; }
        public string Name { get; set; }
        public List<CampfireUser> users { get; set; }
    }

    public class CampfireRoomWrapper
    {
        public CampfireRoom Room { get; set; }
    }


    public class CampfireRoomsWrapper
    {
        public List<CampfireRoom> Rooms { get; set; }
    }
}
