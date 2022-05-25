using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities;
namespace LibraryManagement.Models
{
    public class BooksModel
    {
        public int ISBN { get; set; }
        public string BookName { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int Totalqty { get; set; }

        public int AuthorId1 { get; set; }
        public int AuthorId2 { get; set; }
        public int AuthorId3 { get; set; }

        public List<Author> AuthorsList { get; set; }
        public string Authors { get; set; }

        public string BookStatus { get; set; }
        public bool IsChecked { get; set; }
        public int Enterqty { get; set; }


    }
}