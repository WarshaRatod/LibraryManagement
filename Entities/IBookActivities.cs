using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public interface IBookActivities
    {
        int AddBook(Book book);
        int RemoveBook(int book);
        int UpdateBook(Book book);

        List<Author> Authors();
     
        List<Book> GetAllBooks();
       
    }
}
