using ReviewHall.Core.Models;
using ReviewHall.Core.Entities;
using ReviewHall.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ReviewHall.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _bookService;
        private readonly ReviewService _reviewService;

        public BookController(BookService bookService, ReviewService reviewService)
        {
            _bookService = bookService;
            _reviewService = reviewService;
        }

        // ✅ View all books
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetBooksAsync();
            return View(books);
        }

        // ✅ View book details with reviews
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            var bookEntity = await _bookService.GetBookByIdAsync(id);
            if (bookEntity == null) return NotFound();

            var reviewEntities = await _reviewService.GetReviewsForBookAsync(id);

            var userReviews = reviewEntities.Select(r => new UserReviewViewModel
            {
                ReviewId = r.ReviewId,
                UserName = r.User?.UserName ?? "Unknown User",
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate
            }).ToList();

            var viewModel = new BookListViewModel
            {
                BookId = bookEntity.BookId,
                Title = bookEntity.Title,
                ISBN = bookEntity.ISBN,
                Description = bookEntity.Description,
                Genre = bookEntity.Genre,
                PublishedDate = bookEntity.PublishedDate,
                CoverImageUrl = bookEntity.CoverImageUrl,
                AuthorName = bookEntity.AuthorName,
                AuthorProfileImageUrl = bookEntity.AuthorProfileImageUrl,
                TotalCopies = bookEntity.TotalCopies,
                AvailableCopies = bookEntity.AvailableCopies,
                ReviewCount = bookEntity.ReviewCount,
                AverageRating = bookEntity.AverageRating,
                UserReviews = userReviews
            };

            return PartialView("_BookDetailsPartial", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(Guid bookId, int rating, string comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = "You must be logged in to submit a review." });

                TempData["ErrorMessage"] = "You must be logged in to submit a review.";
                return RedirectToAction("Index"); // 🔥 FIXED
            }

            try
            {
                await _reviewService.AddReviewAsync(bookId, userId, rating, comment);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true, message = "Review submitted successfully!" });

                TempData["SuccessMessage"] = "Review submitted successfully!";
            }
            catch (InvalidOperationException ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = ex.Message });

                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = "An unexpected error occurred while submitting your review." });

                TempData["ErrorMessage"] = "An unexpected error occurred while submitting your review.";
            }

            // 🔥 FIXED: Redirect to /Book instead of /Book/Details
            return RedirectToAction("Index");
        }


        // ✅ Admin-only actions

        [Authorize(Roles = "Admin")]
        public IActionResult AddModal() => PartialView("_AddBookModalPartial");

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _bookService.AddBookAsync(book);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditModal(Guid id)
        {
            var bookEntity = await _bookService.GetBookByIdAsync(id);
            if (bookEntity == null) return NotFound();

            var viewModel = new EditBookViewModel
            {
                BookId = bookEntity.BookId,
                Title = bookEntity.Title,
                ISBN = bookEntity.ISBN,
                Description = bookEntity.Description,
                Genre = bookEntity.Genre,
                PublishedDate = bookEntity.PublishedDate,
                Author = bookEntity.AuthorName,
                AuthorProfileImageUrl = bookEntity.AuthorProfileImageUrl,
                CoverImageUrl = bookEntity.CoverImageUrl
            };

            return PartialView("_EditBookModalPartial", viewModel);
        }

        // File: BookController.cs
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBookViewModel model)
        {
            // 1. Validate the incoming data
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Input validation failed. Please check all fields." });
            }

            // 2. Get the existing book from database
            var existingBook = await _bookService.GetBookByIdAsync(model.BookId);

            if (existingBook == null)
            {
                return NotFound(new { success = false, message = "Book not found." });
            }

            // 3. Update only the editable properties
            existingBook.Title = model.Title;
            existingBook.ISBN = model.ISBN;
            existingBook.Description = model.Description;
            existingBook.Genre = model.Genre;

            // 🔥 FIX: Handle nullable DateTime
            existingBook.PublishedDate = model.PublishedDate ?? existingBook.PublishedDate;

            existingBook.AuthorName = model.Author;
            existingBook.AuthorProfileImageUrl = model.AuthorProfileImageUrl;
            existingBook.CoverImageUrl = model.CoverImageUrl;

            // 4. Save the changes
            var isUpdateSuccessful = await _bookService.UpdateBookAsync(existingBook);

            if (isUpdateSuccessful)
            {
                return Ok(new { success = true, message = "Book updated successfully." });
            }
            else
            {
                return BadRequest(new { success = false, message = "Error saving changes. Please try again." });
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteModal(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();

            return PartialView("_DeleteBookModalPartial", book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();

            await _bookService.DeleteBookAsync(id);
            return RedirectToAction("Index");
        }


    }
}
