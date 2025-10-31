using ReviewHall.Core.Models;
using System;
using System.Collections.Generic;

namespace ReviewHall.Core.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}