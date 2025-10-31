using ReviewHall.Core.Entities;
using ReviewHall.Core.Models;
using ReviewHall.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewHall.Services
{
    public class BookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get all books - FIXED: Rating Computation at Client-Side Sorting
        public async Task<List<BookListViewModel>> GetBooksAsync()
        {
            var booksQuery = _context.Books
                .Include(b => b.Reviews) // Kasama ang Reviews collection
                .Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title ?? string.Empty,
                    ISBN = b.ISBN ?? string.Empty,
                    Description = b.Description ?? string.Empty,
                    Genre = b.Genre ?? string.Empty,
                    PublishedDate = b.PublishedDate,
                    CoverImageUrl = b.CoverImageUrl ?? string.Empty,
                    AuthorName = b.AuthorName ?? string.Empty,
                    AuthorProfileImageUrl = b.AuthorProfileImageUrl ?? string.Empty,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies,

                    // Compute ReviewCount at AverageRating
                    ReviewCount = b.Reviews.Count(),
                    AverageRating = b.Reviews.Any()
                                    ? (decimal)b.Reviews.Average(r => (double)r.Rating)
                                    : 0.0M
                });

            // ✅ FIX: Kumuha muna ng data sa database (ToListAsync)
            var booksList = await booksQuery.ToListAsync();

            // ✅ FIX: I-sort na sa C# memory (Client-side sorting)
            return booksList.OrderByDescending(b => b.AverageRating).ToList();
        }

        // ✅ Get book by ID (Walang pagbabago)
        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            return await _context.Books.FindAsync(id);
        }

        // ✅ Add new book (Walang pagbabago)
        public async Task AddBookAsync(Book book)
        {
            book.BookId = Guid.NewGuid();
            book.ReviewCount = 0;
            book.AverageRating = 0;
            book.IsPublic = true;

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        // File: BookService.cs
        // File: BookService.cs

        // ✅ Tiyakin na ang method na ito ay nagbabalik ng bool
        public async Task<bool> UpdateBookAsync(Book book)
        {
            var existingBook = await _context.Books.FindAsync(book.BookId);

            if (existingBook == null)
            {
                return false;
            }

            try
            {
                // ... (lahat ng existingBook properties ay tama na i-update dito) ...
                existingBook.Title = book.Title;
                existingBook.ISBN = book.ISBN;
                existingBook.Description = book.Description;
                existingBook.Genre = book.Genre;
                existingBook.PublishedDate = book.PublishedDate;
                existingBook.CoverImageUrl = book.CoverImageUrl;
                existingBook.AuthorName = book.AuthorName;
                existingBook.AuthorProfileImageUrl = book.AuthorProfileImageUrl;
                existingBook.TotalCopies = book.TotalCopies;
                existingBook.AvailableCopies = book.AvailableCopies;
                existingBook.IsPublic = true; // Siguraduhin na 'true' ito

                await _context.SaveChangesAsync();
                return true; // Success
            }
            catch (Exception ex)
            {
                // Kung may error sa database (hal. data too long, constraint), babalik ng false
                return false;
            }
        }

        // ✅ Delete book (Walang pagbabago)
        public async Task DeleteBookAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        // ✅ Check if book exists (Walang pagbabago)
        public async Task<bool> BookExistsAsync(Guid id)
        {
            return await _context.Books.AnyAsync(b => b.BookId == id);
        }

        // ✅ Get books by genre - FIXED: Rating Computation at Client-Side Sorting
        public async Task<List<BookListViewModel>> GetBooksByGenreAsync(string genre)
        {
            var booksQuery = _context.Books
                .Include(b => b.Reviews)
                .Where(b => b.Genre == genre)
                .Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title ?? string.Empty,
                    ISBN = b.ISBN ?? string.Empty,
                    Description = b.Description ?? string.Empty,
                    Genre = b.Genre ?? string.Empty,
                    PublishedDate = b.PublishedDate,
                    CoverImageUrl = b.CoverImageUrl ?? string.Empty,
                    AuthorName = b.AuthorName ?? string.Empty,
                    AuthorProfileImageUrl = b.AuthorProfileImageUrl ?? string.Empty,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies,
                    ReviewCount = b.Reviews.Count(),
                    AverageRating = b.Reviews.Any()
                                    ? (decimal)b.Reviews.Average(r => (double)r.Rating)
                                    : 0.0M
                });

            var booksList = await booksQuery.ToListAsync();
            return booksList.OrderByDescending(b => b.AverageRating).ToList();
        }

        // ✅ Search books by title or author - FIXED: Rating Computation at Client-Side Sorting
        public async Task<List<BookListViewModel>> SearchBooksAsync(string searchTerm)
        {
            var booksQuery = _context.Books
                .Include(b => b.Reviews)
                .Where(b => b.Title.Contains(searchTerm) || b.AuthorName.Contains(searchTerm))
                .Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title ?? string.Empty,
                    ISBN = b.ISBN ?? string.Empty,
                    Description = b.Description ?? string.Empty,
                    Genre = b.Genre ?? string.Empty,
                    PublishedDate = b.PublishedDate,
                    CoverImageUrl = b.CoverImageUrl ?? string.Empty,
                    AuthorName = b.AuthorName ?? string.Empty,
                    AuthorProfileImageUrl = b.AuthorProfileImageUrl ?? string.Empty,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies,
                    ReviewCount = b.Reviews.Count(),
                    AverageRating = b.Reviews.Any()
                                    ? (decimal)b.Reviews.Average(r => (double)r.Rating)
                                    : 0.0M
                });

            var booksList = await booksQuery.ToListAsync();
            return booksList.OrderByDescending(b => b.AverageRating).ToList();
        }
    }
}