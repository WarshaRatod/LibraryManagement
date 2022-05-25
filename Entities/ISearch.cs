using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public interface ISearch
    {
        List<Book> GetAllBooks();
        List<Book> GetAvailableBooks(string BookStatus);
        List<Book> GetBooksbyUser(string userID);
    }
}
