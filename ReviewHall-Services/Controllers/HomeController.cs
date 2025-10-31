using ReviewHall.Services;
using ReviewHall.Core.Models;
using Microsoft.AspNetCore.Mvc;
// No need for 'using ReviewHall.Core.Entities;' or complex LINQ here
// since the service should return a fully prepared ViewModel.

namespace ReviewHall.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILibraryService _libraryService;

        public HomeController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        // ✅ Final Cleaned Index Action
        public async Task<IActionResult> Index()
        {
            // 1. Kuhanin ang lahat ng aklat bilang View Models.
            // Ang service na ang bahala mag-compute ng AverageRating at ReviewCount.
            var allBooksWithStats = await _libraryService.GetAllBooksWithReviewsAsync();

            // 2. I-sort ang mga aklat gamit ang AverageRating na galing sa Service.
            // Tinanggal ang b.Reviews computation.
            var sortedBooks = allBooksWithStats
                .OrderByDescending(b => b.AverageRating)
                .ToList();

            // 3. Populate ang HomeViewModel.
            var viewModel = new HomeViewModel
            {
                // Kukunin ang Top 3 books (Highest Rated)
                FeaturedBooks = sortedBooks.Take(3).ToList(),

                // Kukunin ang stats (assuming naka-Async na ito sa service)
                TotalBooks = await _libraryService.GetTotalBookCountAsync(),
                TotalReviews = await _libraryService.GetTotalReviewCountAsync(),
                OverallAverageRating = await _libraryService.GetOverallAverageRatingAsync(),
                TotalMembers = await _libraryService.GetTotalMembersCountAsync()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}