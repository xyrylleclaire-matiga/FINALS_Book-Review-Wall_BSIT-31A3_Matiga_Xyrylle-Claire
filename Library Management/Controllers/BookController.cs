using Library_Management.Models;
using Library_Management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Library_Management.Controllers
{
    // Hindi natin ilalagay ang [Authorize] dito para ang lahat ng user (kahit naka-login lang) ay makakita ng Index.
    public class BookController : Controller
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        // Public: view all books (Makikita ng lahat ng user)
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetBooksAsync();
            return View(books);
        }

        // Admin-only: show Add Book form
        // [Authorize(Roles = "Admin")] ang magre-restrict para Admin lang ang makakapasok.
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
                // Tandaan: Ang code para mag-save sa database ay nasa BookService
                await _bookService.AddBookAsync(model);
                return RedirectToAction("Index");
            }
            // Kung may error sa validation, ibalik ang user sa form
            return View("Add", model); // Ibinabalik ang View na "Add"
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