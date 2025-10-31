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

        public async Task<List<BookListViewModel>> GetBooksAsync()
        {
            var booksQuery = _context.Books
                .Include(b => b.Reviews) 
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

        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task AddBookAsync(Book book)
        {
            book.BookId = Guid.NewGuid();
            book.ReviewCount = 0;
            book.AverageRating = 0;
            book.IsPublic = true;

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            var existingBook = await _context.Books.FindAsync(book.BookId);

            if (existingBook == null)
            {
                return false;
            }

            try
            {
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
                existingBook.IsPublic = true; 

                await _context.SaveChangesAsync();
                return true; 
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task DeleteBookAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> BookExistsAsync(Guid id)
        {
            return await _context.Books.AnyAsync(b => b.BookId == id);
        }

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