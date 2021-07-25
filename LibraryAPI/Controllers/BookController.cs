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
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            this._bookService = bookService;
        }
        
        [HttpPost("NewBook")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> AddBookAsync(AddBookDto book)
        {
            if (book == null) return BadRequest();
            await _bookService.AddBookAsync(book);
            return Created("Book Created Successfully", new { Title = book.Title, ISBN = book.ISBN, PublishYear = book.PublishYear });
        }
       
        [HttpGet("GetBooks")]
        public async Task<ActionResult> GetAllBookAsync()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }
       
        [HttpGet("GetBook")]
        public async Task<ActionResult> GetBook([FromQuery] string bookId)
        {
            if (bookId == null) return BadRequest();
            var book = await _bookService.GetBookByIdAsync(bookId);
            if (book == null) return BadRequest("Book does not Exist");
            return Ok(book);
        }
     
        [HttpGet("BookWithRecord")]
        public async Task<ActionResult> ViewBookWithRecords([FromQuery] string bookId)
        {
            if (bookId == null) return BadRequest();
            var bookWithRecords = await _bookService.GetBookRecordsAsync(bookId);
            List<BookrecordDTO> bookRecord = new List<BookrecordDTO>();
            foreach(var item in bookWithRecords.BookCheckout)
            {
                foreach(var book in item.Books)
                {
                    var bookWithRecord = new BookrecordDTO
                    {
                        Title = book.Title,
                        CoverPrice = book.CoverPrice,
                        DateBorrowed = item.DateCheckedOut,
                        LendersName = $"{item.User.FirstName} {item.User.LastName}",
                        ISBN = book.ISBN,
                        PublishYear = book.PublishYear,
                        BookStatus = item.BookStatus.BookReturnStatus
                    };
                    bookRecord.Add(bookWithRecord);
                }
            }
            return Ok(bookRecord);
        }

        [HttpGet("Search")]
        public async Task<ActionResult> SearchForBook([FromQuery] string searchParams)
        {
            var books = await _bookService.SearchAsync(searchParams);
            return Ok(books);
        }

        [HttpGet("GetBooksWithUsers")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetBooksWithStudent([FromQuery] string checkoutId, [FromQuery] string email)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string errors = default;
            var UserBookToReturn = await _bookService.GetAllBookWithUserAsync(checkoutId, userId, email);
            foreach (var error in UserBookToReturn)
            {
                errors = error.ErrorMessage;
            }
            if (errors != null) return BadRequest(errors);
            return Ok(UserBookToReturn);
        }


        [HttpPost("ReturnBook")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ReturnBook([FromQuery] string checkoutId, [FromQuery]string email)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string errors = default;
            var UserBookToReturn = await _bookService.ReturnAllBookWithUserAsync(checkoutId, userId, email);
            foreach(var error in UserBookToReturn)
            {
                errors = error.ErrorMessage;
            }
            if (errors != null) return BadRequest(errors);
            return Ok(UserBookToReturn);
        }
    }
}
