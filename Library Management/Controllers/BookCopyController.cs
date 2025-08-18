//using Microsoft.AspNetCore.Mvc;

//namespace Library_Management.Controllers
//{
//    public class BookCopyController : Controller
//    {
//        public IActionResult Index(Guid bookId)
//        {
//            var copies = BookService.Instance.GetBookCopies(bookId);
//            return View(copies);
//        }

//        [HttpPost]
//        public IActionResult Add(Guid bookId, string coverImageUrl, string condition, string source)
//        {
//            BookService.Instance.AddBookCopy(bookId, coverImageUrl, condition, source);
//            return Ok();
//        }

//        [HttpPost]
//        public IActionResult Pullout(Guid copyId, string reason)
//        {
//            BookService.Instance.PulloutBookCopy(copyId, reason);
//            return Ok();
//        }
//    }
//}
