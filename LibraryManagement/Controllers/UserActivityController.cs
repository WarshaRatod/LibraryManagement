using DataAccess;
using Entities;
using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagement.Controllers
{
    public class UserActivityController : Controller
    {
        // GET: Register
        public ActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        public ActionResult Register(UserModel objusermodel)
        {
            if (ModelState.IsValid)
            {
                UserActivities userActivities = new UserActivities();
                User user = new User();
                try
                {
                   
                    user.Name = objusermodel.Name;
                    user.Email = objusermodel.Email;
                    user.Role = (int)Role.Member;
                    user = userActivities.Regitration(user);

                    if (user.UserId > 0)
                    {
                        Session["Email"] = user.Email;
                        Session["user"] = user;
                        if (user.Role == 1)
                        {
                            return RedirectToAction("Index", "IssueBook");
                        }
                        else
                        {
                            return RedirectToAction("GetAllBooks", "Book");
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    userActivities = null;
                    user = null;
                }
            }
            return View();
        }
        public ActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                UserActivities userActivities = new UserActivities();
                User user = new User();
                try
                {
                    user.Name = "";
                    user.Email = userModel.Email;
                    user.Role = 0;
                    user = userActivities.Login(user);

                    if (user.UserId > 0)
                    {
                        ViewBag.ErrorMessage = "";
                        Session["Email"] = user.Email;
                        Session["user"] = user;

                        if (user.Role == 1)
                        {
                            return RedirectToAction("Index", "IssueBook");
                        }
                        else
                        {
                            return RedirectToAction("GetAllBooks", "Book");
                        }
                    }
                    else 
                    {
                        ViewBag.ErrorMessage = "User doesn't exists, please register.";
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    userActivities = null;
                    user = null;
                }

            }
            return View("Login");
        }
        [HttpGet]
        public ActionResult LogOut()
        {
            Session["Email"] = null;

            return RedirectToAction("Index", "Home");
        }
        public ActionResult IssueBook(BookIssueModel userModel)
        {
            UserActivities userActivities = new UserActivities();
            User user = new User();
            return View();
        }


    }
}