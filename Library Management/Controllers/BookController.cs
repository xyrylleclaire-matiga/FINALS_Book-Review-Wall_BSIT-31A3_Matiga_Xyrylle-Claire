using Library_Management.Models;
using Library_Management_Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            var books = BookService.Instance.GetBooks();
            return View(books);
        }

        public IActionResult AddModal()
        {
            return PartialView("_AddBookPartial");
        }

        [HttpPost]
        public IActionResult Add(AddBookViewModel vm)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            BookService.Instance.AddBook(vm);

            return Ok();
        }


        public IActionResult EditModal(Guid id)
        {
            var editBookViewModel = BookService.Instance.GetBookById(id);
            if (editBookViewModel == null) return NotFound();


            return PartialView("_EditBookPartial", editBookViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditBookViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BookService.Instance.UpdateBook(vm);

            return Ok();
        }

        public IActionResult DeleteModal(Guid id)
        {
            var book = BookService.Instance.GetBookById(id);
           
            return PartialView("_DeletePartial", id);
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            try
            {
                BookService.Instance.DeleteBook(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


        public IActionResult Details(Guid id)
        {
            var book = BookService.Instance.GetBooks().First(b => b.BookId == id);
            return View(book);
        }

        
        



    }
}