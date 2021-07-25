using Library.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class BookToReturnWIthDetails
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishYear { get; set; }
        [Column(TypeName = "Money")]
        public decimal CoverPrice { get; set; }
        public string BookAvalability { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public virtual List<BookCheckout> BookCheckout { get; set; }
        public string BookCheckoutId { get; set; }
        public ApplicationUser User { get; set; }
        public string BookStatus { get; set; }
    }
}
