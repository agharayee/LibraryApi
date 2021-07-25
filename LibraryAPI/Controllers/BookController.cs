using LibraryAPI.Dtos;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            this._bookService = bookService;
            this._logger = logger;
        }
        
        [HttpPost("NewBook")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> AddBookAsync(AddBookDto book)
        {
            if (book == null) return BadRequest();
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _bookService.AddBookAsync(book,adminId);
            _logger.LogInformation($"AddBookAsync EndPoint was accessed on {DateTime.Now}");
            return Created("Book Created Successfully", new { Title = book.Title, ISBN = book.ISBN, PublishYear = book.PublishYear });
        }
       
        [HttpGet("GetBooks")]
        public async Task<ActionResult> GetAllBookAsync()
        {
            var books = await _bookService.GetAllBooksAsync();
            _logger.LogInformation($"GetBooks EndPoint was accessed on {DateTime.Now}");
            return Ok(books);
        }
       
        [HttpGet("GetBook")]
        public async Task<ActionResult> GetBook([FromQuery] string bookId)
        {
            if (bookId == null) return BadRequest();
            var book = await _bookService.GetBookByIdAsync(bookId);
            if (book == null) return BadRequest("Book does not Exist");
            _logger.LogInformation($"GetBook EndPoint was accessed on {DateTime.Now}");
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
            _logger.LogInformation($"ViewBooksWithRecords EndPoint was accessed on {DateTime.Now}");
            return Ok(bookRecord);
        }

        [HttpGet("Search")]
        public async Task<ActionResult> SearchForBook([FromQuery] string searchParams)
        {
            var books = await _bookService.SearchAsync(searchParams);
            _logger.LogInformation($"Search EndPoint was accessed on {DateTime.Now}");
            return Ok(books);
        }

        [HttpGet("GetBooksWithUser")]
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
            _logger.LogInformation($"GetBooksWithUser EndPoint was accessed on {DateTime.Now}");
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
            _logger.LogInformation($"ReturnBook EndPoint was accessed on {DateTime.Now}");
            return Ok(UserBookToReturn);
        }
    }
}
