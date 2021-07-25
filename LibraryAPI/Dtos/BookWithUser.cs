using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class BookWithUser
    {
        public string BookTitle { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime SupposedReturnDate { get; set; }
        public DateTime InitialReturnDate { get; set; }
        public decimal LateReturnPenalty { get; set; }
        public string BorrowerName { get; set; }
        public int LateReturnDays { get; set; }
        public string ErrorMessage { get; set; }
        public int NumberOfBookWithUser { get; set; }
    }
}
