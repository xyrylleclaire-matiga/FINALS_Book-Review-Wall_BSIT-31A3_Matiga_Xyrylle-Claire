using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ReviewHall.Core.Models
{
    public class BookListViewModel
    {
        [Key]
        public Guid BookId { get; set; }
        public string? Title { get; set; } = default!;
        public string? ISBN { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string? Genre { get; set; } = default!;
        public DateTime? PublishedDate { get; set; } = default!;
        public string? CoverImageUrl { get; set; } = default!;

        public string? AuthorName { get; set; } = default!;
        public string? AuthorProfileImageUrl { get; set; } = default!;
        public int TotalCopies { get; set; } = 0;
        public int AvailableCopies { get; set; } = 0;

        public int ReviewCount { get; set; } = 0;
        public decimal AverageRating { get; set; } = 0.0M;

        public List<UserReviewViewModel> UserReviews { get; set; } = new List<UserReviewViewModel>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

    }
}
