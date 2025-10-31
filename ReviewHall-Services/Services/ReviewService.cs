using Microsoft.EntityFrameworkCore;
using ReviewHall.Core.Entities;
using ReviewHall.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewHall.Services
{
    public class ReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get all reviews for a specific book (with User data)
        public async Task<List<Review>> GetReviewsForBookAsync(Guid bookId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.BookId == bookId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        // ✅ Get a specific review by ID (with Book + User)
        public async Task<Review?> GetReviewByIdAsync(Guid reviewId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        // ✅ Add a new review (object-based)
        public async Task AddReviewAsync(Review review)
        {
            // Safety check: validate foreign keys
            bool bookExists = await _context.Books.AnyAsync(b => b.BookId == review.BookId);
            bool userExists = await _context.Users.AnyAsync(u => u.Id == review.UserId);

            if (!bookExists)
                throw new InvalidOperationException("Book not found — cannot add review.");

            if (!userExists)
                throw new InvalidOperationException("User not found — cannot add review.");

            review.ReviewId = Guid.NewGuid();
            review.ReviewDate = DateTime.UtcNow;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        // ✅ Add a new review (parameter-based, for controller use)
        public async Task AddReviewAsync(Guid bookId, string userId, int rating, string comment)
        {
            // Safety check: validate foreign keys
            bool bookExists = await _context.Books.AnyAsync(b => b.BookId == bookId);
            bool userExists = await _context.Users.AnyAsync(u => u.Id == userId);

            if (!bookExists)
                throw new InvalidOperationException("Book not found — cannot add review.");

            if (!userExists)
                throw new InvalidOperationException("User not found — cannot add review.");

            var review = new Review
            {
                ReviewId = Guid.NewGuid(),
                BookId = bookId,
                UserId = userId,
                Rating = rating,
                Comment = comment,
                ReviewDate = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        // ✅ Update existing review
        public async Task UpdateReviewAsync(Review review)
        {
            var existingReview = await _context.Reviews.FindAsync(review.ReviewId);

            if (existingReview == null)
                throw new InvalidOperationException("Review not found.");

            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;
            existingReview.ReviewDate = DateTime.UtcNow;

            _context.Reviews.Update(existingReview);
            await _context.SaveChangesAsync();
        }

        // ✅ Delete a review by ID
        public async Task DeleteReviewAsync(Guid reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Review not found — nothing to delete.");
            }
        }
    }
}
