using ReviewHall.Services;
using ReviewHall.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ReviewHall.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILibraryService _libraryService;

        public HomeController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        public async Task<IActionResult> Index()
        {
            var allBooksWithStats = await _libraryService.GetAllBooksWithReviewsAsync();

            var sortedBooks = allBooksWithStats
                .OrderByDescending(b => b.AverageRating)
                .ToList();

            var viewModel = new HomeViewModel
            {
                FeaturedBooks = sortedBooks.Take(3).ToList(),

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