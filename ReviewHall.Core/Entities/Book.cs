// File: Book.cs

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
        [Required] // Ang description ay pwedeng long string (walang MaxLength)
        public string Description { get; set; } = default!;
        [Required, MaxLength(100)]
        public string Genre { get; set; } = default!;
        public DateTime PublishedDate { get; set; }

        // ⭐ FIX: Dagdagan ang MaxLength para suportahan ang mahabang URL
        [Required, MaxLength(500)]
        public string CoverImageUrl { get; set; } = default!;

        [Required, MaxLength(100)]
        public string AuthorName { get; set; } = default!;

        // ⭐ FIX: Dagdagan ang MaxLength para suportahan ang mahabang URL
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