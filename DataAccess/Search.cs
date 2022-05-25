using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DataAccess
{
    class Search:ISearch
    {
        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();
            string s = "SELECT * FROM Books";
            SqlUtilityClass.ExecuteReader(s);
            return books;
        }

        public List<Book> GetAvailableBooks(string BookStatus)
        {
            List<Book> books = new List<Book>();
            return books;
        }

        public List<Book> GetBooksbyUser(string userID)
        {
            List<Book> books = new List<Book>();
            string s = "SELECT * FROM Books";
            SqlUtilityClass.ExecuteReader(s);
            return books;
        }
       
    }
}
