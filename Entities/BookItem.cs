using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class BookItem:Book
    {
        public int BookitemId { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime DueDate  { get; set; }
    }
}
