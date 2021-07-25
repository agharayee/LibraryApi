using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Entities
{
    public class BookCheckout
    {
        public string Id { get; set; }
        public virtual List<Book> Books { get; set; } = new List<Book>();
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public DateTime DateCheckedOut { get; set; }
        public DateTime ProposedReturnDate { get; set; }
        public DateTime InitialReturnDate { get; set; }
        [Column(TypeName = "Money")]
        public decimal LateReturnPenaltyFees { get; set; }
        public BookStatus BookStatus { get; set; }
        public string BookStatusId { get; set; }
        public string StaffThatCheckedInBook { get; set; }
        public string StaffThatCheckedOutBook { get; set; }
    }
}
