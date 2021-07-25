using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class CheckOutBook
    {
        public string BookCheckoutId { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }
        public decimal CoverPrice { get; set; }
        public string Borrower { get; set; }
        public DateTime DateToBeReturn { get; set; }
        public string StaffThatAdministerBook { get; set; }
    }
}
