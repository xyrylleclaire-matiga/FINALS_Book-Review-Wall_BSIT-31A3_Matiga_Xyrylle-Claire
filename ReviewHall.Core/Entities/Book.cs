
using System.ComponentModel.DataAnnotations;

namespace ReviewHall.Core.Entities
{
    public class Book
    {
        [Key]
        public Guid BookId { get; set; }
        [Required, MaxLength(250)]
        public string Title { get; set; } = default!;
        [Required, MaxLength(50)]
        public string ISBN { get; set; } = default!;
        [Required] 
        public string Description { get; set; } = default!;
        [Required, MaxLength(100)]
        public string Genre { get; set; } = default!;
        public DateTime PublishedDate { get; set; }

        [Required, MaxLength(500)]
        public string CoverImageUrl { get; set; } = default!;

        [Required, MaxLength(100)]
        public string AuthorName { get; set; } = default!;

        [Required, MaxLength(500)]
        public string AuthorProfileImageUrl { get; set; } = default!;

        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int ReviewCount { get; set; }
        public decimal AverageRating { get; set; }
        public bool IsPublic { get; set; } = true;

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}