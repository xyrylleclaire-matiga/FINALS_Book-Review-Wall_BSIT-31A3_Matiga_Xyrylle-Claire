using Library_Management.Models;
using Library_Management.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Library_Management.Controllers
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
            // Get all books with reviews
            var allBooks = await _libraryService.GetAllBooksWithReviewsAsync();

            IEnumerable<BookListViewModel> filteredBooks;

            // Admin sees all books
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                filteredBooks = allBooks.Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    Description = b.Description,
                    Genre = b.Genre,
                    AuthorName = b.AuthorName,
                    CoverImageUrl = b.CoverImageUrl
                }).ToList();
            }
            else
            {
                // Non-admin users and guests see only public books
                // If you don’t have IsPublic yet, everyone sees all books for now
                filteredBooks = allBooks.Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    Description = b.Description,
                    Genre = b.Genre,
                    AuthorName = b.AuthorName,
                    CoverImageUrl = b.CoverImageUrl
                }).ToList();
            }

            var viewModel = new HomeViewModel
            {
                FeaturedBooks = filteredBooks.Take(3).ToList(),
                TotalBooks = filteredBooks.Count(),
                TotalReviews = _libraryService.GetTotalReviewCount(),
                OverallAverageRating = _libraryService.GetOverallAverageRating(),
                TotalMembers = _libraryService.GetTotalMembersCount()
            };

            return View(viewModel);
        }
    }
}
