using DataAccess;
using Entities;
using LibraryManagement.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LibraryManagement.Controllers
{
    public class BookController : Controller
    {
        BookActivities bookActivities = new BookActivities();

    
        //Get All Books
        public ActionResult GetAllBooks(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageSize = SqlUtilityClass.PageSize;
            int pageNumber = 0;
            List<BooksModel> lstbookModel = new List<BooksModel>();
            List<BooksModel> modelList = null;
            try
            {
                List<Book> lstbook = bookActivities.GetAllBooks();

               
                foreach (Book book in lstbook)
                {
                    BooksModel booksModel = new BooksModel();
                    booksModel.BookName = book.BookName;
                    booksModel.Publisher = book.Publisher;
                    booksModel.Title = book.Title;
                    booksModel.ISBN = book.ISBN;
                    booksModel.Totalqty = book.Totalqty;
                    booksModel.BookStatus = book.BookStatus == 0 ? "Not Available" : "Available";
                    booksModel.Authors = book.Authors;
                    lstbookModel.Add(booksModel);
                }

                Session["AllBookDetails"] = lstbookModel;
                ViewData["CurrentSort"] = sortOrder;
                ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Title_desc" : "";
                ViewData["AuthorsSortParm"] = sortOrder == "Authors" ? "Authors_desc" : "Authors";


                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                 modelList = lstbookModel;


                if (!String.IsNullOrEmpty(searchString) && searchString == "Available")
                {
                    modelList = modelList.Where(s => s.BookStatus.Equals(searchString)).ToList<BooksModel>();
                }
                else if (!String.IsNullOrEmpty(searchString) && searchString == "Book taken by User")
                {
                    modelList = modelList.Where(s => s.BookStatus.Equals("Not Available")).ToList<BooksModel>();
                }

                switch (sortOrder)
                {
                    case "Title_desc":
                        modelList = modelList.OrderByDescending(s => s.Title).ToList<BooksModel>();
                        break;
                    case "Authors_desc":
                        modelList = modelList.OrderByDescending(s => s.Authors).ToList<BooksModel>(); ;
                        break;
                    case "Authors":
                        modelList = modelList.OrderBy(s => s.Authors).ToList<BooksModel>(); ;
                        break;
                    default:  // Name ascending 
                        modelList = modelList.OrderBy(s => s.Title).ToList<BooksModel>(); ;
                        break;
                }

                 pageSize = 6;
                 pageNumber = (page ?? 1);
            }
            catch (Exception)
            {

                throw;
            }
          
            return View(modelList.ToPagedList(pageNumber, pageSize));

        }

        // GET: Book/Create
        public ActionResult AddNewBook()
        {
            ViewData["VBAuthorList"] = new MultiSelectList(bookActivities.Authors(), "AuthorId", "AuthorName");
            Session.Add("VBAuthorList", (MultiSelectList)ViewData["VBAuthorList"]);

            return View();
        }

        // POST: Book/Create
        [HttpPost]
        public ActionResult AddNewBook(BooksModel booksmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Book book = new Book();
                    book.ISBN = booksmodel.ISBN;
                    book.BookName = booksmodel.BookName;
                    book.Title = booksmodel.Title;
                    book.Publisher = booksmodel.Publisher;
                    book.Totalqty = booksmodel.Totalqty;
                    book.AuthorsList = new List<Author>();

                    ViewData["VBAuthorList"] = (MultiSelectList)Session["VBAuthorList"];

                    if (!string.IsNullOrEmpty(Request.Form["ddAuthorId"]))
                    {
                        string selectedAuthorids = Request.Form["ddAuthorId"].ToString();
                        string[] Authorids = selectedAuthorids.Split(',');
                        if (Authorids.Length > 0)
                            for (int i = 0; i < Authorids.Length; i++)
                            {
                                book.AuthorsList.Add(new Author { AuthorId = Convert.ToInt32(Authorids[i]) });
                            }
                       
                    }
                    int result = bookActivities.AddBook(book);

                    if (result > 0)
                    {
                        
                        return RedirectToAction("GetAllBooks");

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return View();
        }

        // GET: Book/Edit/5
        public ActionResult Edit(BooksModel booksModel)
        {

            return View(booksModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        public ActionResult Edit(int ISBN, BooksModel booksmodelUpdate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Book book = new Book();
                    book.ISBN = ISBN;
                    book.BookName = booksmodelUpdate.BookName;
                    book.Title = booksmodelUpdate.Title;
                    book.Publisher = booksmodelUpdate.Publisher;
                    book.Totalqty = booksmodelUpdate.Totalqty;
                    int result = bookActivities.UpdateBook(book);
                    if (result > 0)
                    {

                        return RedirectToAction("GetAllBooks", "Book");

                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        // GET: Book/Delete/5


        // POST: Book/Delete/5

        [HttpGet]
        public ActionResult Delete(BooksModel booksModel)
        {
            return View(booksModel);
        }

        [HttpPost]
        public ActionResult Delete(int ISBN)
        {
            try
            {
                int result = bookActivities.RemoveBook(ISBN);
                if (result > 0)
                {
                    return RedirectToAction("GetAllBooks", "Book");
                }
            }
            catch
            {

            }
            return View();
        }

        [HttpGet]
        //[Route("Search")]
        [Route("Search/{id?}")]
        public ActionResult Search(string search)
        {
            try
            {


                return RedirectToAction("GetAllBooks", "Book", search);

            }
            catch
            {

            }
            return View();
        }

    }
}
