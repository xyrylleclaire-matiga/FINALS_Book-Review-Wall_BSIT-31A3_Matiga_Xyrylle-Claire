using Library_Management.Data;
using Library_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library_Management.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly BookService _bookService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LibraryService(ApplicationDbContext context, BookService bookService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _bookService = bookService;
            _userManager = userManager;
        }

        // ✅ Get all books with reviews
        public async Task<List<BookListViewModel>> GetAllBooksWithReviewsAsync()
        {
            var books = await _bookService.GetBooksAsync();
            return books.ToList();
        }

        // ✅ Get total review count across all books
        public int GetTotalReviewCount()
        {
            return _context.Books.Sum(b => b.ReviewCount);
        }

        // ✅ Get overall average rating across all books
        public decimal GetOverallAverageRating()
        {
            var booksWithReviews = _context.Books
                .Where(b => b.ReviewCount > 0)
                .Select(b => b.AverageRating)
                .ToList();

            if (booksWithReviews.Count == 0)
                return 0;

            return Math.Round((decimal)booksWithReviews.Average(), 2);
        }

        // ✅ Get total members (from ASP.NET Identity)
        public int GetTotalMembersCount()
        {
            return _userManager.Users.Count();
        }
    }
}
