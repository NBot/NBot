using System;
using NBot.Core;

namespace NBot.Campfire
{
    public class User : IEntity
    {
        public string EmailAddress { get; set; }
        public bool Admin { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Type { get; set; }
        public string AvatarUrl { get; set; }

        #region IEntity Members

        public int Id { get; set; }
        public string Name { get; set; }

        #endregion
    }
}