using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Book
    {
        public int ISBN { get; set; }
        public string BookName { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int Totalqty { get; set; }

        public List<Author> AuthorsList { get; set; }
        public string Authors { get; set; }

        public int BookStatus { get; set; }
    }
}
