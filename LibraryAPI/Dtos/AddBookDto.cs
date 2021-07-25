using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class AddBookDto
    {
        [Required(ErrorMessage ="Title is Required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "ISBN is Required")]
        public string ISBN { get; set; }
        [Required(ErrorMessage = "Publish Year is Required")]
        [DataType(DataType.Date)]
        public DateTime PublishYear { get; set; }
        [Column(TypeName = "Money")]
        [Required(ErrorMessage = "Cover Price is Required")]
        public decimal CoverPrice { get; set; }
        [Required(ErrorMessage = "IsAvailable is Required")]
        public bool IsAvailable { get; set; }
       
    }
}
