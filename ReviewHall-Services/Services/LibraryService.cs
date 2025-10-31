using ReviewHall.Infrastructure.Data;
using ReviewHall.Core.Models;
using ReviewHall.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq; // Kailangan para sa LINQ
using System.Threading.Tasks; // Kailangan para sa Task<T>
using System.Collections.Generic; // Kailangan para sa List<T>
using System; // Kailangan para sa Math.Round

namespace ReviewHall.Services
{
    // Tiyakin na ang ILibraryService ay dineklara na may Task<List<BookListViewModel>>
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

        // --- Core Data Methods ---

        // ✅ FIXED: Tiyakin na ang return type ay EKSKATO na Task<List<BookListViewModel>>
        public async Task<List<BookListViewModel>> GetAllBooksWithReviewsAsync()
        {
            // Dahil inayos na natin ang BookService.GetBooksAsync() para magbalik ng List<BookListViewModel>,
            // direktang tawag lang ang kailangan.
            return await _bookService.GetBooksAsync();
        }

        // --- Statistics Methods ---

        // 1. ✅ Implement GetTotalBookCountAsync()
        public async Task<int> GetTotalBookCountAsync()
        {
            return await _context.Books.CountAsync();
        }

        // 2. ✅ Implement GetTotalReviewCountAsync()
        public async Task<int> GetTotalReviewCountAsync()
        {
            return await _context.Reviews.CountAsync();
        }

        // 3. ✅ Implement GetOverallAverageRatingAsync() - SQLite compatible
        public async Task<decimal> GetOverallAverageRatingAsync()
        {
            // Gumamit ng double casting para maiwasan ang SQLite NotSupportedException sa Average
            var averageRatingAsDouble = await _context.Reviews
                .Where(r => r.Rating > 0)
                // I-cast ang Rating (int) sa double?
                .AverageAsync(r => (double?)r.Rating);

            // I-convert ang double result pabalik sa decimal, i-round off sa 2 decimal places
            if (averageRatingAsDouble.HasValue)
            {
                return Math.Round((decimal)averageRatingAsDouble.Value, 2);
            }

            return 0.0M;
        }

        // 4. ✅ Implement GetTotalMembersCountAsync()
        public async Task<int> GetTotalMembersCountAsync()
        {
            return await _userManager.Users.CountAsync();
        }
    }
}