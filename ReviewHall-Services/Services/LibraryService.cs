using ReviewHall.Infrastructure.Data;
using ReviewHall.Core.Models;
using ReviewHall.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq; 
using System.Threading.Tasks; 
using System.Collections.Generic; 
using System; 

namespace ReviewHall.Services
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

        public async Task<List<BookListViewModel>> GetAllBooksWithReviewsAsync()
        {
            return await _bookService.GetBooksAsync();
        }

        public async Task<int> GetTotalBookCountAsync()
        {
            return await _context.Books.CountAsync();
        }

        public async Task<int> GetTotalReviewCountAsync()
        {
            return await _context.Reviews.CountAsync();
        }

        public async Task<decimal> GetOverallAverageRatingAsync()
        {
            var averageRatingAsDouble = await _context.Reviews
                .Where(r => r.Rating > 0)
                .AverageAsync(r => (double?)r.Rating);

            if (averageRatingAsDouble.HasValue)
            {
                return Math.Round((decimal)averageRatingAsDouble.Value, 2);
            }

            return 0.0M;
        }

        public async Task<int> GetTotalMembersCountAsync()
        {
            return await _userManager.Users.CountAsync();
        }
    }
}