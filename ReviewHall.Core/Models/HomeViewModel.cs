namespace ReviewHall.Core.Models
{
    public class HomeViewModel
    {
        public List<BookListViewModel> FeaturedBooks { get; set; } = new();
        public int TotalBooks { get; set; }
        public int TotalReviews { get; set; }
        public decimal OverallAverageRating { get; set; }
        public int TotalMembers { get; set; }
    }
}