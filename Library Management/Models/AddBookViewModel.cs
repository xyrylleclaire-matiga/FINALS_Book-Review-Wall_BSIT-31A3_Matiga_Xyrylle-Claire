namespace Library_Management.Models
{
    public class AddBookViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ISBN { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public DateTime? PublishedDate { get; set; }

        public string? CoverImageUrl { get; set; }
        public string? Condition { get; set; }
        public string? Source { get; set; }

        public string? Author { get; set; }
        public string? AuthorProfileImageUrl { get; set; }
        public int TotalCopies { get; set; } = 0;
        public int AvailableCopies { get; set; } = 0;

    }
}
