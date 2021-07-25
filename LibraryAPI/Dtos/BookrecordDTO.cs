using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class BookrecordDTO
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishYear { get; set; }
        [Column(TypeName = "Money")]
        public decimal CoverPrice { get; set; }
        public string BookStatus { get; set; }
        public string LendersName { get; set; }
        public DateTime DateBorrowed { get; set; }

    }
}
