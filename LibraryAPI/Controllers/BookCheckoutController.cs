using LibraryAPI.Dtos;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class BookCheckoutController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookCheckoutController(IBookService bookService)
        {
            this._bookService = bookService;
        }
        [HttpPost("BookCheckout")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> BookCheckoutAsync([FromBody] List<string> booksId, [FromQuery]string email)
        {
            CheckOutBook book = default;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (booksId.Count == 0) return BadRequest("No Book Selected to checkout");
            var booktoCheckout = await _bookService.CheckOutBook(booksId, userId, email);
            var booklist = new List<CheckOutBook>();
            foreach(var item in booktoCheckout.BooksToCheckout)
            {
                book = new CheckOutBook
                {
                    BookCheckoutId = booktoCheckout.Id,
                    BookTitle = item.Title,
                    BookId = item.Id,
                    CoverPrice = item.CoverPrice,
                    DateToBeReturn = booktoCheckout.DateToBeReturned,
                    StaffThatAdministerBook = booktoCheckout.StaffAssignedTheBook
                   
                };
                book.Borrower = booktoCheckout.NameOfBorrower;
                
                booklist.Add(book);
            }
            
            return Ok(booklist);
        }
    }
}
