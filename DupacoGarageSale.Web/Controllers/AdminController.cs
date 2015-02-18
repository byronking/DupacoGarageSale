﻿using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Repository;
using DupacoGarageSale.Data.Services;
using DupacoGarageSale.Domain.Helpers;
using DupacoGarageSale.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DupacoGarageSale.Web.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();
                viewModel.AdminUser = session.User;

                var repository = new AdminRepository();

                // Get the count of user accounts.
                viewModel.UserCount = repository.GetCountOfRegisteredUsers();
                
                // Get the count of garage sales.
                viewModel.GarageSaleCount = repository.GetCountOfGarageSales();

                // Get the counts of items and categories.
                viewModel.SpecialItemsCount = repository.GetItemsAndCategoriesCount();

                // Get the time/date of the first created account.


                // Get the time/date of the last created account.

                 

                ViewBag.NavAdmin = "active";
                return View(viewModel);
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        #region Garage sale users
        public ActionResult Users()
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();
                viewModel.AdminUser = session.User;

                // Load all the users.
                var repository = new AdminRepository();
                viewModel.Users = repository.GetAllGarageSaleUsers();

                Session["AllUsers"] = viewModel.Users;

                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

                // Show the success message if the sale worked.
                if (Session["ChangePasswordSuccessful"] != null)
                {
                    var saveSuccessful = Convert.ToBoolean(Session["ChangePasswordSuccessful"]);

                    if (saveSuccessful)
                    {
                        ViewBag.Invisible = "false";
                    }
                }

                // Clear the session object.
                Session["SaveSuccessful"] = null;

                Session["ViewModel"] = viewModel;

                ViewBag.NavAdmin = "active";
                return View(viewModel);
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        [HttpPost]
        public ActionResult SearchUsers(AdminViewModel model)
        {
            if (Session["UserSession"] != null)
            {
                var criteria = model.SearchCriteria;

                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();
                viewModel.AdminUser = session.User;

                // Load all the users.
                var repository = new AdminRepository();
                viewModel.Users = repository.GetGarageSaleUsersForSearch(criteria);

                Session["AllUsers"] = viewModel.Users;

                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

                Session["ViewModel"] = viewModel;

                ViewBag.NavAdmin = "active";
                return View("Users", viewModel);
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        public ActionResult ClearUserSearch()
        {
            return RedirectToAction("Users");
        }

        public ActionResult EditUser(int userId)
        {
            if (Session["UserSession"] != null)
            {
                var viewModel = new AdminViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as AdminViewModel;
                }

                if (Session["AllUsers"] != null)
                {
                    viewModel.Users = Session["AllUsers"] as List<GarageSaleUser>;
                }

                var repository = new AccountsRepository();
                var user = repository.GetUserProfileInfoById(userId);
                viewModel.User = user;

                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");
                ViewData["ShowEditUser"] = "true";

                // Show the success message if the sale worked.
                if (Session["SaveSuccessful"] != null)
                {
                    var saveSuccessful = Convert.ToBoolean(Session["SaveSuccessful"]);

                    if (saveSuccessful)
                    {
                        ViewBag.Invisible = "false";
                    }
                }

                // Clear the session object.
                Session["SaveSuccessful"] = null;

                return View("Users", viewModel);
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        /// <summary>
        /// This saves changes to the registered user profile.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveUserProfile(HttpPostedFileBase fileUpload, GarageSaleUser user)
        {
            user.ModifyDate = DateTime.Now;
            user.ModifyUser = user.UserName;

            if (fileUpload != null)
            {
                // Save the image file.
                user.ProfilePicLink = fileUpload.FileName;

                var fileName = Path.GetFileName(user.ProfilePicLink);
                var dir = ConfigurationManager.AppSettings["ProfileImagesDirectory"].ToString();

                var storageDir = dir + Path.DirectorySeparatorChar + fileName;

                if (!System.IO.File.Exists(fileName))
                {
                    fileUpload.SaveAs(dir + Path.DirectorySeparatorChar + fileName);
                }
            }
            else
            {
                // Use the default pic.
                user.ProfilePicLink = "keep-calm-and-come-to-the-dupaco-garage-sale.png";
            }

            var errors = ModelState.Where(v => v.Value.Errors.Any());

            var saveResult = new UserSaveResult();
            var repository = new AccountsRepository();
            saveResult = repository.SaveGarageSaleUserProfile(user);

            if (saveResult.IsSaveSuccessful)
            {
                Session["SaveSuccessful"] = true;
            }

            return RedirectToAction("EditUser", new RouteValueDictionary(new
            {
                controller = "Admin",
                action = "EditUser",
                userId = user.UserId
            }));
        }

        #endregion

        #region Garage sales

        public ActionResult GarageSales()
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();

                viewModel.AdminUser = session.User;

                // Load all the garage sales.
                var repository = new AdminRepository();
                viewModel.GarageSales = repository.GetAllGarageSales();

                Session["AllGarageSales"] = viewModel.GarageSales;

                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

                Session["ViewModel"] = viewModel;

                ViewBag.NavAdmin = "active";
                return View(viewModel);
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        public ActionResult SearchGarageSales(AdminViewModel model)
        {
            if (Session["UserSession"] != null)
            {
                var criteria = model.SearchCriteria;
                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();

                viewModel.AdminUser = session.User;

                // Load all the garage sales.
                var repository = new AdminRepository();
                viewModel.GarageSales = repository.GetGarageSalesForSearch(criteria);

                Session["AllGarageSales"] = viewModel.GarageSales;

                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

                Session["ViewModel"] = viewModel;

                ViewBag.NavAdmin = "active";
                return View("GarageSales", viewModel);
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        public ActionResult ViewGarageSale(int saleId)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();

                viewModel.AdminUser = session.User;

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as AdminViewModel;
                }

                if (Session["AllGarageSales"] != null)
                {
                    viewModel.GarageSales = Session["AllGarageSales"] as List<GarageSale>;
                }

                var repository = new GarageSaleRepository();
                viewModel.GarageSale = repository.GetGarageSaleAndItemsById(saleId);
                viewModel.GarageSaleSpecialItems = repository.GetGarageSaleSpecialItems(saleId);
                
                // Get the messages.
                viewModel.GarageSaleMessages = repository.GetGarageSaleMessages(saleId);

                // Get the blog posts.
                var blogRepo = new BlogPostRepository();
                viewModel.BlogPosts = blogRepo.GetBlogPosts(viewModel.GarageSale.GarageSaleId);                

                Session["ViewModel"] = viewModel;

                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");
                ViewData["ShowEditGarageSale"] = "true";

                return View("GarageSales", viewModel); 
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        public ActionResult ClearSalesSearch()
        {
            return RedirectToAction("GarageSales");
        }

        #endregion

        public ActionResult GarageSaleItems()
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();
                viewModel.AdminUser = session.User;

                ViewBag.NavAdmin = "active";
                return View(viewModel);
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        public ActionResult ChangePassword(FormCollection form)
        {
            var password = form["User.Password"].ToString();

            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                session.User.Password = password;

                // Update the user's password.
                var repository = new AccountsRepository();
                var hashedPassword = AccountHelper.GetSHA1Hash(session.User.UserName, password);

                try
                {
                    var saveSuccessful = repository.UpdateUserPassword(session.User.UserName, hashedPassword);

                    if (saveSuccessful)
                    {
                        Session["ChangePasswordSuccessful"] = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.ToString());
                }

                return RedirectToAction("Users", new RouteValueDictionary(new
                {
                    controller = "Admin",
                    action = "Users",
                    userId = session.User.UserId
                }));
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        /// <summary>
        /// This gets all the blog posts for the admin view.
        /// </summary>
        /// <returns></returns>
        public ActionResult BlogPosts()
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();

                viewModel.AdminUser = session.User;

                // Load all the blog posts.
                var repository = new AdminRepository();
                viewModel.BlogPosts = repository.GetAllBlogPosts();

                Session["AllBlogPosts"] = viewModel.BlogPosts;
                Session["ViewModel"] = viewModel;
                ViewData["ShowBlogPost"] = "true";

                ViewBag.NavAdmin = "active";
                return View(viewModel);
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        /// <summary>
        /// This gets a blog post by id.
        /// </summary>
        /// <param name="blog_post_id"></param>
        /// <returns></returns>
        public ActionResult ViewBlogPost(int blog_post_id)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();

                viewModel.AdminUser = session.User;

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as AdminViewModel;

                    // Get the blog post.
                    var blogRepo = new AdminRepository();
                    viewModel.BlogPost = blogRepo.GetBlogPost(blog_post_id);
                    Session["ViewModel"] = viewModel;
                }

                if (Session["AllBlogPosts"] != null)
                {
                    viewModel.BlogPosts = Session["AllBlogPosts"] as List<BlogPost>;
                }

                ViewData["ShowBlogPost"] = "true";

                return View("BlogPosts", viewModel); 
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }
    }
}
