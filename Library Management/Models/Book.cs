using System.ComponentModel.DataAnnotations;

namespace Library_Management.Models
{
    public class Book
    {
        [Key]
        public Guid BookId { get; set; }
        public string Title { get; set; } = default!;
        public string ISBN { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Genre { get; set; } = default!;
        public DateTime PublishedDate { get; set; }
        public string CoverImageUrl { get; set; } = default!;
        public string AuthorName { get; set; } = default!;
        public string AuthorProfileImageUrl { get; set; } = default!;
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int ReviewCount { get; set; }
        public decimal AverageRating { get; set; }
        public bool IsPublic { get; set; } = true;

    }
}
