using Entities;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryManagement.Models;
using PagedList;
using System.Text;

namespace LibraryManagement.Controllers
{
    public class IssueBookController : Controller
    {
        BookActivities bookActivities = new BookActivities();

        // GET: IssueBook
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageSize = 6;
            int pageNumber = 0;
            ViewBag.errorMessage = string.Empty;
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
                    booksModel.Authors = book.Authors;
                    booksModel.BookStatus = book.BookStatus == 0 ? "Not Available" : "Available";
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


                if (!String.IsNullOrEmpty(searchString))
                {
                    modelList = modelList.Where(s => s.Title.Contains(searchString)).ToList<BooksModel>();
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
                        modelList = modelList.OrderBy(s => s.BookName).ToList<BooksModel>(); ;
                        break;
                }

                pageSize = 6;
                pageNumber = (page ?? 1);

            }
            catch (Exception)
            {

                throw;
            }

            ViewBag.errorMessage = "";

            return View(modelList.ToPagedList(pageNumber, pageSize));
            // return View(modelList);

        }

        [HttpPost]
        public JsonResult Index(List<BooksModel> obj)
        {
            bool jsonresult = false;
            try
            {

                if (obj != null && obj.Count > 0)
                {
                    StringBuilder ISBNs = new StringBuilder();
                    StringBuilder Quantities = new StringBuilder();
                    Dictionary<int, int> ISBNQtykeyValuePairs = new Dictionary<int, int>();
                    foreach (BooksModel bm in obj)
                    {
                        ISBNQtykeyValuePairs.Add(bm.ISBN, bm.Totalqty);
                        ISBNs.Append(bm.ISBN);
                        ISBNs.Append(",");
                    }
                    //Get available items and filter for user entered quantity
                    BookIssue bookIssueItem = bookActivities.GetAvailbeBooksItemsByISBN(ISBNs.ToString(), ISBNQtykeyValuePairs);

                    if (bookIssueItem.BookItems.Count > 0)
                    {
                        BookIssueModel bookIssueModel = new BookIssueModel();
                        User objuser = new User();
                        if (Session["user"] != null)
                        {
                            objuser = (User)Session["user"];
                            bookIssueModel.UserId = objuser.UserId;
                            bookIssueModel.Email = objuser.Email;
                            bookIssueModel.Name = objuser.Name;
                        }
                        bookIssueModel.BookItems = bookIssueItem.BookItems;
                        Session["UserCheckedBooks"] = bookIssueModel;

                        jsonresult = true;
                    }
                }
                else
                {
                    //return Re
                }
            }

            catch (Exception)
            {

                throw;
            }
            return Json(jsonresult);

        }


        [HttpGet]
        public ActionResult Checkout()
        {
            BookIssueModel bookIssueModel = new BookIssueModel();

            if (Session["UserCheckedBooks"] != null)
            {
                bookIssueModel = (BookIssueModel)Session["UserCheckedBooks"];
            }
            Session["ConfirmItem"] = bookIssueModel;
            User objuser = new User();

            if (Session["user"] != null)
            {
                objuser = (User)Session["user"];
                ViewBag.UserName = objuser.Name;
                ViewBag.Email = objuser.Email;
                ViewBag.UserID = objuser.UserId;
                ViewBag.ReferenceNo = string.Empty;
            }

            return View("Checkout", bookIssueModel);

        }

        [HttpPost]
        public ActionResult Checkout(BookIssueModel booksModel)
        {
            UserActivities userActivities = new UserActivities();
            BookIssue bookIssue = new BookIssue();
            BookIssueModel bookIssueModel = new BookIssueModel();
            string refno = "";

            if (Session["ConfirmItem"] != null)
            {
                bookIssueModel = (BookIssueModel)Session["UserCheckedBooks"];

                bookIssue.UserId = bookIssueModel.UserId;
                bookIssue.Email = bookIssueModel.Email;
                bookIssue.Name = bookIssueModel.Name;
                bookIssue.BookItems = bookIssueModel.BookItems;

                refno = userActivities.IssueBook(bookIssue);
                if (refno != string.Empty)
                {
                    ViewBag.ReferenceNo = "Request Sumbited successfully and your reference number is:" + refno;
                }
                else
                {
                    ViewBag.ReferenceNo = "Error Occured while submitting your request";

                }

            }
            return View("Checkout", bookIssueModel);
        }
    }
}