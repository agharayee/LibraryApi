using AutoMapper;
using Library.Data.Data;
using Library.Data.Entities;
using LibraryAPI.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkingDaysManagement;

namespace LibraryAPI.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        public async Task AddBookAsync(AddBookDto book, string adminId)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            var admin = await _userManager.FindByIdAsync(adminId);
            var bookToBeCreated = _mapper.Map<Book>(book);
            bookToBeCreated.Id = Guid.NewGuid().ToString();
            bookToBeCreated.DateCreated = DateTime.Now;
            bookToBeCreated.CreatedBy = $"{admin.FirstName} {admin.LastName}";
            bookToBeCreated.CreatedById = admin.Id;
            await _context.Books.AddAsync(bookToBeCreated);
            await _context.SaveChangesAsync();
        }

        public Task EditBookAsync(Book book)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BookToReturnDto>> GetAllBooksAsync()
        {
           var books = await _context.Books.OrderBy(b => b.PublishYear).OrderBy(p => p.PublishYear).ToListAsync();
            var bookToReturn = _mapper.Map<List<BookToReturnDto>>(books);
            foreach(var book in bookToReturn)
            {
                string IsAvailable = default;
                if (book.IsAvailable == true) IsAvailable = "Available";
                else IsAvailable = "Not Available at the Moment";
                book.BookAvalability = IsAvailable;
              
            }
            return bookToReturn;
        }

        public async Task<BookToReturnDto> GetBookByIdAsync(string bookId)
        {
            if (bookId == null) throw new ArgumentNullException(nameof(bookId));
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null) return null;
            string IsAvailable = default;
            if (book.IsAvailable == true) IsAvailable = "Available";
            else IsAvailable = "Not Available at the Moment";
            var bookToReturn = _mapper.Map<BookToReturnDto>(book);
            bookToReturn.BookAvalability = IsAvailable;
            return bookToReturn;
        }

        public async Task<List<BookToReturnDto>> SearchAsync(string searchParam)
        {
            if (string.IsNullOrEmpty(searchParam))
            {
                var book = await GetAllBooksAsync();
                foreach(var item in book)
                {
                    string IsAvailable = default;
                    if (item.IsAvailable == true) IsAvailable = "Available";
                    else IsAvailable = "Not Available at the Moment";
                    var helper = new WorkingDayHelper();
                    var booksToReturn = new BookToReturnDto
                    {
                        BookId = item.BookId,
                        DateCreated = item.DateCreated,
                        DateModified = helper.FuturWorkingDays(DateTime.Now, 3),
                        CoverPrice = item.CoverPrice,
                        IsAvailable = item.IsAvailable,
                        BookAvalability = IsAvailable,
                        ISBN = item.ISBN,
                        PublishYear = item.PublishYear,
                        Title = item.Title
                    };
                }
                
                
            }
            var collection = _context.Books as IQueryable<Book>;
            collection = collection.AsQueryable().Where(b => b.IsAvailable == true || b.Title.Contains(searchParam) ||
                                                         b.ISBN.Contains(searchParam)).OrderBy(c => c.PublishYear);
            var books = await collection.ToListAsync();

            var bookToReturn = _mapper.Map<IEnumerable<BookToReturnDto>>(books);
            foreach(var book in bookToReturn)
            {
                string IsAvailable = default;
                if (book.IsAvailable == true) IsAvailable = "Available";
                else IsAvailable = "Not Available at the Moment";
                book.BookAvalability = IsAvailable;
            }
            return bookToReturn.ToList();
        }

        public async Task<BookToReturnWIthDetails> GetBookRecordsAsync(string bookId)
        {
            var book = await _context.Books.Include(b => b.BookCheckout).FirstOrDefaultAsync(book => book.Id == bookId);
            List<BookCheckout> bookCheckouts = new List<BookCheckout>();
            foreach(var item in book.BookCheckout)
            {
                var user = await _userManager.FindByIdAsync(item.UserId);
                item.User = user;
                var books = await _context.BookStatus.FirstOrDefaultAsync(b => b.Id == item.BookStatusId);
                item.BookStatus = books;
            }
            string IsAvailable = default;
      
            if (book.IsAvailable == true) IsAvailable = "Available";
            else IsAvailable = "Not Available at the Moment";
            
            var bookToReturn = new BookToReturnWIthDetails
            {
                Id = book.Id,
                DateCreated = book.DateCreated,
                DateModified = book.DateModified,
                CoverPrice = book.CoverPrice,
                IsAvailable = book.IsAvailable,
                BookAvalability = IsAvailable,
                BookCheckout = book.BookCheckout,
                ISBN = book.ISBN,
                PublishYear = book.PublishYear,
                Title = book.Title, 
            
            };
            return bookToReturn;
        }

        public async Task<BookCheckoutDto> CheckOutBook(List<string> bookIds, string adminId, string userEmail)
        {
            //if (bookIds == null) return null;
            List<Book> BookToCheckOut = new List<Book>();
            var user = await _userManager.FindByEmailAsync(userEmail);
            if(user == null)
            {
                var errorMessage = new BookCheckoutDto
                {
                    ErrorMessage = "No User Found"
                };
                return errorMessage;
            }
            var admin = await _userManager.FindByIdAsync(adminId);
            if (admin == null)
            {
                var errorMessage = new BookCheckoutDto
                {
                    ErrorMessage = "No User Found"
                };
                return errorMessage;
            }
            foreach (var item in bookIds)
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == item);
                BookToCheckOut.Add(book);
            }
            var helper = new WorkingDayHelper();
            var checkout = new BookCheckout
            {
                Id = Guid.NewGuid().ToString(),
                Books = BookToCheckOut,
                DateCheckedOut = DateTime.Now,
                ProposedReturnDate = helper.FuturWorkingDays(DateTime.Now, 10),
                User = user,
                UserId = user.Id,
                BookStatus = new BookStatus
                {
                    BookReturnStatus = "Borrowed",
                    Id = Guid.NewGuid().ToString(),
                    IsReturned = false
                },
                StaffThatCheckedOutBook = $"{admin.FirstName} {admin.LastName}",


            };
            var checkoutToReturn = new BookCheckoutDto
            {
                Id = checkout.Id,
                BooksToCheckout = checkout.Books,
                DateBorrowed = checkout.DateCheckedOut,
                DateToBeReturned = checkout.ProposedReturnDate,
                NameOfBorrower = $"{checkout.User.FirstName} {checkout.User.LastName}",
                StaffAssignedTheBook = $"{admin.FirstName} {admin.LastName}"
            };

            await _context.BookCheckouts.AddAsync(checkout);
            await _context.SaveChangesAsync();
            return checkoutToReturn;
        }

        public async Task<List<BookWithUser>> GetAllBookWithUserAsync(string checkoutId, string userId, string userEmail)
        {
            List<BookWithUser> userBook = new List<BookWithUser>();
            var booksWithUser = await GetAllBookWithUser(checkoutId, userId, userEmail);
            foreach (var item in booksWithUser.Books)
            {

                var book = new BookWithUser
                {
                    BorrowedDate = booksWithUser.DateCheckedOut,
                    BorrowerName = $"{booksWithUser.User.FirstName} {booksWithUser.User.LastName}",
                    InitialReturnDate = DateTime.Now,
                    SupposedReturnDate = booksWithUser.ProposedReturnDate,
                    BookTitle = item.Title,
                    NumberOfBookWithUser = booksWithUser.Books.Count(),
                };
                userBook.Add(book);
            }
            return userBook;
        }
       

        public async Task<List<BookWithUser>> ReturnAllBookWithUserAsync(string checkoutId, string userId, string userEmail)
        {
            List<BookWithUser> userBook = new List<BookWithUser>();
            var helper = new WorkingDayHelper();
            BookWithUser book = new BookWithUser();
            DateTime lateCalculatedWorkingDay = default;
            var admin = await _userManager.FindByIdAsync(userId);
            var booksWithUser =  await GetAllBookWithUser(checkoutId, userId, userEmail);
            decimal payment = default;
            var init = DateTime.Now;
            var lateDay = init - booksWithUser.ProposedReturnDate;
            if (lateDay.Days > 0)
            {
                lateCalculatedWorkingDay = helper.PastWorkingDays(init, lateDay.Days);
                payment = 200 * lateCalculatedWorkingDay.Day;
                book.LateReturnPenalty = payment;
            }
            else
            {

                payment = 0.0M;
                book.LateReturnPenalty = payment;
            }
            foreach (var item in booksWithUser.Books)
            {

                book = new BookWithUser
                {
                    BorrowedDate = booksWithUser.DateCheckedOut,
                    BorrowerName = $"{booksWithUser.User.FirstName} {booksWithUser.User.LastName}",
                    InitialReturnDate = DateTime.Now,
                    SupposedReturnDate = booksWithUser.ProposedReturnDate,
                    LateReturnDays = lateCalculatedWorkingDay.Day,
                    BookTitle = item.Title,
                    LateReturnPenalty = payment,
                };
                userBook.Add(book);
            }
            var id = booksWithUser.BookStatus.Id;
            var bookStatus = await _context.BookStatus.FirstOrDefaultAsync(b => b.Id == id);
            bookStatus.BookReturnStatus = "Returned";
            booksWithUser.StaffThatCheckedInBook = $"{admin.FirstName} {admin.LastName}";
            bookStatus.IsReturned = true;
            booksWithUser.InitialReturnDate = DateTime.Now;
            booksWithUser.LateReturnPenaltyFees = payment;
            await _context.SaveChangesAsync();
            return userBook;
        }


        private async Task<BookCheckout> GetAllBookWithUser(string checkoutId, string userId, string userEmail)
        {
            BookWithUser book = new BookWithUser();
            var user = await _userManager.FindByEmailAsync(userEmail);
            var admin = await _userManager.FindByIdAsync(userId);
            if (user == null || admin == null)
            {
                var noUserFound = new BookWithUser
                {
                    ErrorMessage = "No User Found"
                };
                throw new ArgumentNullException(nameof(userId));
            }
            List<BookWithUser> userBook = new List<BookWithUser>();
            var booksWithUser = await _context.BookCheckouts.Include(b => b.Books).Include(b => b.BookStatus).FirstOrDefaultAsync(b => (b.Id == checkoutId) && (b.BookStatus.IsReturned == false));
            if (booksWithUser == null)
            {
                List<BookWithUser> tBook = new List<BookWithUser>();
                var noBookInUserPossesion = new BookWithUser
                {
                    ErrorMessage = "No Book In User Possesion"
                };
                tBook.Add(noBookInUserPossesion);
                throw new ArgumentNullException(nameof(userId));
            }
            booksWithUser.User = user;
            return booksWithUser;
        }
    }
}
