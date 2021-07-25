using Library.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class BookCheckoutDto
    {
        public string Id { get; set; }
        public List<Book> BooksToCheckout { get; set; }
        public DateTime DateBorrowed { get; set; }
        public DateTime DateToBeReturned { get; set; }
        public string NameOfBorrower { get; set; }
        public string ErrorMessage { get; set; }
        public string StaffAssignedTheBook { get;  set; }
    }
}
