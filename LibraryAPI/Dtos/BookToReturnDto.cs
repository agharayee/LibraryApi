using Library.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class BookToReturnDto
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishYear { get; set; }
        public string BookAvalability { get; set; }
        [Column(TypeName = "Money")]
        public decimal CoverPrice { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
      
    }
}
