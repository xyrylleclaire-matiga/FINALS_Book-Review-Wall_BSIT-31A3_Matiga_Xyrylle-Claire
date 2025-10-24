using Library_Management.Models;
using Library_Management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Library_Management.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        // Public: view all books
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetBooksAsync();
            return View(books);
        }

        // Admin-only: show Add Book form
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        // Admin-only: POST Add Book
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _bookService.AddBookAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // Admin-only: Delete book
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookService.DeleteBookAsync(id);
            return RedirectToAction("Index");
        }
    }
}
