using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DataAccess
{
    public class BookActivities : IBookActivities
    {
        public List<Book> GetAllBooks()
        {
            List<Book> booklist = new List<Book>();
            SqlDataReader rdrbook = SqlUtilityClass.ExecuteReader("GetAllBooks");
            int bookStatus;

            while (rdrbook.Read())
            {
                bookStatus = 0;
                if (rdrbook["BookStatus"] != null)
                {
                   int.TryParse(rdrbook["BookStatus"].ToString(), out bookStatus);
                }
                booklist.Add(new Book
                {
                    ISBN = Convert.ToInt32(rdrbook["ISBN"]),
                    BookName = rdrbook["BookName"].ToString(),
                    Title = rdrbook["Title"].ToString(),
                    Publisher = rdrbook["Publisher"].ToString(),
                    Totalqty = Convert.ToInt32(rdrbook["Quantity"]),
                    Authors = Convert.ToString(rdrbook["Authors"]),
                    BookStatus = bookStatus
                });
            }


            return booklist;
        }
        public int AddBook(Book book)
        {
            SqlParameter[] sqlParameters = SetupAddNewBookParameters(book);
            int result = SqlUtilityClass.ExecuteNonQuery(sqlParameters, "Addbooks");
            return result;

        }
        public int RemoveBook(int book)
        {
            SqlParameter[] sqlParameters = SetupDeleteBookParameters(book);
            int result = SqlUtilityClass.ExecuteNonQuery(sqlParameters, "RemoveBook");

            return result;
        }
        private SqlParameter[] SetupDeleteBookParameters(int SBIN)
        {
            SqlParameter[] sqlParameters = new SqlParameter[1];

            SqlParameter sqlParameterISBN = new SqlParameter("@ISBN", SBIN);
            sqlParameterISBN.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[0] = sqlParameterISBN;
            return sqlParameters;
        }

        public int UpdateBook(Book book)
        {
            SqlParameter[] sqlParameters = SetupUpdateBookParameters(book);
            int result = SqlUtilityClass.ExecuteNonQuery(sqlParameters, "UpdateBook");
            return result;
        }

        public List<Author> Authors()
        {
            List<Author> authorlist = new List<Author>();
            SqlDataReader rdrAuthor = SqlUtilityClass.ExecuteReader("GetAllAuthors");

            while (rdrAuthor.Read())
            {
                authorlist.Add(new Author
                {
                    AuthorId = Convert.ToInt32(rdrAuthor["AuthorId"]),
                    AuthorName = rdrAuthor["AuthorName"].ToString(),
                    
                });
            }
            return authorlist;
        }

        public bool IssueBook(Book book, User user)
        {
            return true;
        }

        private SqlParameter[] SetupGetAvailbeBooksItemsByISBNParameters(string SBIN)
        {
            SqlParameter[] sqlParameters = new SqlParameter[1];

            SqlParameter sqlParameterISBN = new SqlParameter("@ISBNList", SBIN);
            sqlParameterISBN.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[0] = sqlParameterISBN;
            return sqlParameters;
        }
        public BookIssue GetAvailbeBooksItemsByISBN(string ISBN, Dictionary<int,int> keyValuePairs)
        {
            SqlParameter[] sqlParameters = SetupGetAvailbeBooksItemsByISBNParameters(ISBN);
            SqlDataReader rdrbook = SqlUtilityClass.ExecuteReader("GetAvailbeBooksItemsByISBN", sqlParameters);
            int bookStatus;

            BookIssue bookIssue = new BookIssue();

            bookIssue.BookItems = new List<BookItem>();

            while (rdrbook.Read())
            {
                bookStatus = 0;
                if (rdrbook["Status"] != null)
                {
                    int.TryParse(rdrbook["Status"].ToString(), out bookStatus);
                }
                //add only required quantites
                if (keyValuePairs.ContainsKey(Convert.ToInt32(rdrbook["ISBN"])))
                {
                    int qty = 0;
                    keyValuePairs.TryGetValue(Convert.ToInt32(rdrbook["ISBN"]), out qty);

                    if (qty > 0)
                    {

                        BookItem bi = new BookItem();

                        bi.BookName = rdrbook["BookName"].ToString();
                        bi.Title = rdrbook["Title"].ToString();
                        bi.Publisher = rdrbook["Publisher"].ToString();
                        bi.BookitemId = Convert.ToInt32(rdrbook["BookId"]);
                        bi.ISBN = Convert.ToInt32(rdrbook["ISBN"]);
                        bi.BookStatus = bookStatus;
                        bi.BorrowedDate = DateTime.Now;
                        bi.DueDate = DateTime.Now.AddDays(15);
                       
                        bookIssue.BookItems.Add(bi);

                        keyValuePairs[Convert.ToInt32(rdrbook["ISBN"])] = qty - 1;
                    }
                        //booklist.Add(bookIssue);
                }
            }

            return bookIssue;
        }


        private SqlParameter[] SetupAddNewBookParameters(Book book)
        {
            SqlParameter[] sqlParameters = new SqlParameter[6];

            SqlParameter sqlParameterISBN = new SqlParameter("@ISBN", book.ISBN);
            sqlParameterISBN.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[0] = sqlParameterISBN;

            SqlParameter sqlParameterBookName = new SqlParameter("@BookName", book.BookName);
            sqlParameterBookName.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[1] = sqlParameterBookName;

            SqlParameter sqlParameterTitle = new SqlParameter("@Title", book.Title);
            sqlParameterTitle.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[2] = sqlParameterTitle;

            SqlParameter sqlParameterPublisher = new SqlParameter("@Publisher", book.Publisher);
            sqlParameterPublisher.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[3] = sqlParameterPublisher;

            SqlParameter sqlParameterTotalqty  = new SqlParameter("@Totalqty", book.Totalqty);
            sqlParameterTotalqty.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[4] = sqlParameterTotalqty;

            StringBuilder sb = new StringBuilder();
            if (book.AuthorsList != null && book.AuthorsList.Count > 0)
            {
                foreach(Author aut in book.AuthorsList)
                {
                    sb.Append(aut.AuthorId.ToString());
                    sb.Append(",");
                }
            }
            SqlParameter sqlParameterAuthorIds = new SqlParameter("@Authors", sb.ToString());
            sqlParameterAuthorIds.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[5] = sqlParameterAuthorIds;

            return sqlParameters;
        }

        private SqlParameter[] SetupUpdateBookParameters(Book book)
        {
            SqlParameter[] sqlParameters = new SqlParameter[6];

            SqlParameter sqlParameterISBN = new SqlParameter("@ISBN", book.ISBN);
            sqlParameterISBN.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[0] = sqlParameterISBN;

            SqlParameter sqlParameterBookName = new SqlParameter("@BookName", book.BookName);
            sqlParameterBookName.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[1] = sqlParameterBookName;

            SqlParameter sqlParameterTitle = new SqlParameter("@Title", book.Title);
            sqlParameterTitle.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[2] = sqlParameterTitle;

            SqlParameter sqlParameterPublisher = new SqlParameter("@Publisher", book.Publisher);
            sqlParameterPublisher.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[3] = sqlParameterPublisher;

            SqlParameter sqlParameterTotalqty = new SqlParameter("@Totalqty", book.Totalqty);
            sqlParameterTotalqty.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[4] = sqlParameterTotalqty;

            StringBuilder sb = new StringBuilder();
            if (book.AuthorsList != null && book.AuthorsList.Count > 0)
            {
                foreach (Author aut in book.AuthorsList)
                {
                    sb.Append(aut.AuthorId.ToString());
                    sb.Append(",");
                }
            }
            SqlParameter sqlParameterAuthorIds = new SqlParameter("@Authors", sb.ToString());
            sqlParameterAuthorIds.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[5] = sqlParameterAuthorIds;

            return sqlParameters;
        }
    }
}
