using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Entities
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishYear { get; set; }
        [Column(TypeName ="Money")]
        public decimal CoverPrice { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public DateTime DateModified { get; set; }
        public virtual List<BookCheckout> BookCheckout { get; set; }
        public string BookCheckoutId { get; set; }
    }
}
