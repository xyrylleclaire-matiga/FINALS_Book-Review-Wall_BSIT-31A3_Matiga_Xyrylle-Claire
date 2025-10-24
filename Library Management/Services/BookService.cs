using Library_Management.Models;
using Library_Management.Data;
using Microsoft.EntityFrameworkCore;

namespace Library_Management.Services
{
    public class BookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Add new book
        public async Task AddBookAsync(AddBookViewModel book)
        {
            // Convert ViewModel → Entity
            var newBook = new Book
            {
                BookId = Guid.NewGuid(),
                Title = book.Title,
                ISBN = book.ISBN,
                Description = book.Description,
                Genre = book.Genre,
                PublishedDate = book.PublishedDate ?? DateTime.Now,
                CoverImageUrl = book.CoverImageUrl,
                AuthorName = book.Author,
                AuthorProfileImageUrl = book.AuthorProfileImageUrl,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                ReviewCount = 0,
                AverageRating = 0
            };

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();
        }

        // ✅ Get all books (Entity → ViewModel)
        public async Task<IEnumerable<BookListViewModel>> GetBooksAsync()
        {
            return await _context.Books
                .Select(b => new BookListViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    Description = b.Description,
                    Genre = b.Genre,
                    PublishedDate = b.PublishedDate,
                    CoverImageUrl = b.CoverImageUrl,
                    AuthorName = b.AuthorName,
                    AuthorProfileImageUrl = b.AuthorProfileImageUrl,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies,
                    ReviewCount = b.ReviewCount,
                    AverageRating = b.AverageRating
                })
                .ToListAsync();
        }

        // ✅ Delete book
        public async Task DeleteBookAsync(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
                throw new KeyNotFoundException("Book not found.");

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
