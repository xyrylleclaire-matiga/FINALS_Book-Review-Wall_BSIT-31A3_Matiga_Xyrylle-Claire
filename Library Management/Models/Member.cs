using System;
using System.ComponentModel.DataAnnotations;

namespace Library_Management.Models
{
    public class Member
    {
        [Key]
        public Guid MemberId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.Now;
    }
}
