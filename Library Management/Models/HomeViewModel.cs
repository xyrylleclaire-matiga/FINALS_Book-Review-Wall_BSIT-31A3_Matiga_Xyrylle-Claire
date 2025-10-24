// HomeViewModel.cs
using Library_Management.Models;
using Library_Management_Domain.Entities;
using System.Collections.Generic;

namespace Library_Management.Models
{
    public class HomeViewModel
    {
        public List<BookListViewModel> FeaturedBooks { get; set; } = new List<BookListViewModel>();

        public int TotalBooks { get; set; }
        public int TotalMembers { get; set; }

        // Ito ang bagong Review stats na pumalit sa AvailableCopies
        public int TotalReviews { get; set; }
        public decimal OverallAverageRating { get; set; }
    }
}