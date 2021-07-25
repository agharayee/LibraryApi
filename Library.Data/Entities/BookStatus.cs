using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Entities
{
    public class BookStatus
    {
        public string Id { get; set; }
        public string BookReturnStatus { get; set; }
        public bool IsReturned { get; set; }
        

    }
}
