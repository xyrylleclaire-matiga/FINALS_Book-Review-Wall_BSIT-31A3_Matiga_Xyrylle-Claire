using System;

namespace ReviewHall.Core.Models
{
    public class ReviewViewModel
    {
        public Guid ReviewId { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        public string UserName { get; set; }
        public string BookTitle { get; set; }
    }

    public class UserReviewViewModel
    {
        public Guid ReviewId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}