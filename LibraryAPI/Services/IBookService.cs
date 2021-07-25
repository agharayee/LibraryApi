using Library.Data.Entities;
using LibraryAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public interface IBookService
    {
        Task AddBookAsync(AddBookDto book);
        Task EditBookAsync(Book book);
        Task<List<BookToReturnDto>> GetAllBooksAsync();
        Task<BookToReturnDto> GetBookByIdAsync(string bookId);
        Task<List<BookToReturnDto>> SearchAsync(string searchParam);
        Task<BookToReturnWIthDetails> GetBookRecordsAsync(string bookId);
        Task<List<BookWithUser>> GetAllBookWithUserAsync(string checkoutId, string userId, string userEmail);
        Task<List<BookWithUser>> ReturnAllBookWithUserAsync(string checkoutId, string userId, string userEmail);
        Task<BookCheckoutDto> CheckOutBook(List<string> bookIds, string adminId, string userEmail);

    }
}
