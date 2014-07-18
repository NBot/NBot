using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBot.Core;

namespace NBot.CampfireAdapter
{
    public class CampfireUser : IEntity
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string created_at { get; set; }
        public bool Admin { get; set; }
        public string api_auth_token { get; set; }
        public string email_address { get; set; }
        public string Name { get; set; }
        public string avatar_url { get; set; }
    }

    public class CampfireUserWrapper
    {
        public CampfireUser User { get; set; }
    }

    public class CampfireUsersWrapper
    {
        public List<CampfireUser> Users { get; set; } 
    }
}
