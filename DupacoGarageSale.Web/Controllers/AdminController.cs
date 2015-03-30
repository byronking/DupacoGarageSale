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
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DupacoGarageSale.Web.Controllers
{
    public class AdminController : Controller
    {
        /// <summary>
        /// This loads the admin main page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserSession"] != null)
            {
                // Need to check if the user is an admin. If not, send them to the login screen.

                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();
                viewModel.AdminUser = session.User;

                var repository = new AdminRepository();

                var userIsAdmin = repository.VerifyUserIsAdmin(viewModel.AdminUser.UserName);

                if (!userIsAdmin)
                {
                    return View("~/Views/Accounts/Login.cshtml");
                }

                // Get the count of user accounts.
                viewModel.UserCount = repository.GetCountOfRegisteredUsers();
                
                // Get the count of garage sales.
                viewModel.GarageSaleCount = repository.GetCountOfGarageSales();

                // Get the counts of items and categories.
                viewModel.SpecialItemsCount = repository.GetItemsAndCategoriesCount();

                // Get the time/date of the first created account.

                // Get the time/date of the last created account.

                ViewBag.OverviewActive = "active";
                ViewBag.NavAdmin = "active";
                return View(viewModel);
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        #region Garage sale users
        /// <summary>
        /// This loads the garage sale users page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Users()
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();
                viewModel.AdminUser = session.User;

                // Load all the users.
                var repository = new AdminRepository();
                viewModel.Users = repository.GetAllUsersWithAddresses();

                Session["AllUsers"] = viewModel.Users;

                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

                // Show the success message if the save worked.
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

                Session["AdminViewModel"] = viewModel;

                ViewBag.UsersActive = "active";
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

                Session["AdminViewModel"] = viewModel;

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

        /// <summary>
        /// This loads a user for editing.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult EditUser(int userId)
        {
            if (Session["UserSession"] != null)
            {
                var viewModel = new AdminViewModel();

                if (Session["AdminViewModel"] != null)
                {
                    viewModel = Session["AdminViewModel"] as AdminViewModel;
                }

                if (Session["AllUsers"] != null)
                {
                    viewModel.Users = Session["AllUsers"] as List<GarageSaleUser>;
                }

                var repository = new AccountsRepository();
                var user = repository.GetUserProfileInfoById(userId);
                viewModel.User = user;

                // Get the user's garage sales.
                var garageSaleRepository = new GarageSaleRepository();
                viewModel.UserGarageSales = garageSaleRepository.GetGarageSaleByUserName(viewModel.User.UserName);

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

        /// <summary>
        /// This allows for changing of user passwords.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult ChangePassword(FormCollection form)
        {
            var userName = form["User.UserName"].ToString();
            var password = form["User.Password"].ToString();

            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                session.User.Password = password;

                // Update the user's password.
                var repository = new AccountsRepository();
                var hashedPassword = AccountHelper.GetSHA1Hash(userName, password);

                try
                {
                    var saveSuccessful = repository.UpdateUserPassword(userName, hashedPassword);

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

        #endregion

        #region Garage sales
        /// <summary>
        /// This loads the garage sales page.
        /// </summary>
        /// <returns></returns>
        public ActionResult GarageSales()
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();

                if (Session["AdminViewModel"] != null)
                {
                    viewModel = Session["AdminViewModel"] as AdminViewModel;
                }

                viewModel.AdminUser = session.User;

                // Load all the garage sales.
                var repository = new AdminRepository();
                viewModel.GarageSales = repository.GetAllGarageSales();

                Session["AllGarageSales"] = viewModel.GarageSales;

                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

                Session["AdminViewModel"] = viewModel;

                ViewBag.GarageSalesActive = "active";
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

                Session["AdminViewModel"] = viewModel;

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

                if (Session["AdminViewModel"] != null)
                {
                    viewModel = Session["AdminViewModel"] as AdminViewModel;
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

                Session["AdminViewModel"] = viewModel;

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

        /// <summary>
        /// Edit a garage sale.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditGarageSale(int id)
        {
            var userSession = new UserSession();

            if (Session["UserSession"] != null)
            {
                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

                userSession = Session["UserSession"] as UserSession;

                var repository = new GarageSaleRepository();

                var viewModel = new AdminViewModel
                {
                    AdminUser = userSession.User,
                    GarageSale = repository.GetGarageSaleAndItemsById(id),
                    SelectedCategories = new List<int>()
                };

                if (Session["AdminViewModel"] != null)
                {
                    var savedViewModel = Session["AdminViewModel"] as AdminViewModel;
                    viewModel.GarageSales = savedViewModel.GarageSales;

                    if (viewModel.GarageSales == null)
                    {
                        // Load all the garage sales by default.
                        var adminRepository = new AdminRepository();
                        viewModel.GarageSales = adminRepository.GetAllGarageSales();
                    }
                }
                else
                {
                    // Load all the garage sales by default.
                    var adminRepository = new AdminRepository();
                    viewModel.GarageSales = adminRepository.GetAllGarageSales();
                }

                var accountRepository = new AccountsRepository();
                viewModel.User = accountRepository.GetGarageSaleUserByUserName(viewModel.GarageSale.ModifyUser);

                foreach (var itemId in viewModel.GarageSale.GarageSaleItems)
                {
                    viewModel.SelectedCategories.Add(itemId.ItemSubcategoryId);
                }

                // Get the categories and subcategories.
                viewModel.ItemCategories = repository.GetCategoriesAndSubcategories();

                // Get the special items.
                viewModel.GarageSaleSpecialItems = repository.GetGarageSaleSpecialItems(viewModel.GarageSale.GarageSaleId);
                foreach (var itemId in viewModel.GarageSaleSpecialItems)
                {
                    viewModel.SelectedCategories.Add(itemId.ItemSubcategoryId);
                }

                var selectedCategories = viewModel.SelectedCategories.ToArray();
                ViewBag.SelectedCategories = string.Join(",", selectedCategories);

                // Get the blog posts.
                var blogRepo = new BlogPostRepository();
                viewModel.BlogPosts = blogRepo.GetBlogPosts(viewModel.GarageSale.GarageSaleId);

                // Get the messages.
                viewModel.GarageSaleMessages = repository.GetGarageSaleMessages(id);

                // Show the success message if saving the garage sale worked.
                if (Session["SaveSuccessful"] != null)
                {
                    var saveSuccessful = Convert.ToBoolean(Session["SaveSuccessful"]);

                    if (saveSuccessful)
                    {
                        ViewBag.Invisible = "false";
                    }
                }

                ViewData["ShowEditGarageSale"] = "true";

                // Save the viewmodel for later use.
                Session["AdminViewModel"] = viewModel;

                // Clear the session object.
                Session["SaveSuccessful"] = null;

                ViewBag.NavGarageSales = "active";
                return View("GarageSales", viewModel); 
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        public ActionResult ClearSalesSearch()
        {
            return RedirectToAction("GarageSales");
        }        

        /// <summary>
        /// This loadsthe garage sale items page.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// This updates a user's garage sale.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateGarageSale(HttpPostedFileBase garageSalePicUpload, AdminViewModel model, FormCollection form)
        {
            var categoryIdList = new List<int>();
            model.GarageSale.GarageSaleItems = new List<GarageSaleItem>();

            foreach (var key in form.AllKeys)
            {
                int categoryId;

                if (Int32.TryParse(key, out categoryId))
                {
                    var garageSaleItem = new GarageSaleItem
                    {
                        SaleId = model.GarageSale.GarageSaleId,
                        ItemSubcategoryId = categoryId
                    };
                    
                    model.GarageSale.GarageSaleItems.Add(garageSaleItem);
                    categoryIdList.Add(categoryId);
                }
            }

            var viewModel = new AdminViewModel();

            if (Session["AdminViewModel"] != null)
            {
                viewModel = Session["AdminViewModel"] as AdminViewModel;
                //model.GarageSale.GarageSaleItems = viewModel.GarageSale.GarageSaleItems;
                //model.GarageSaleSpecialItems = viewModel.GarageSaleSpecialItems;
            }

            if (garageSalePicUpload != null)
            {
                // Save the image file.
                model.GarageSale.GargeSalePicLink = garageSalePicUpload.FileName;

                var fileName = Path.GetFileName(model.GarageSale.GargeSalePicLink);
                var dir = ConfigurationManager.AppSettings["GarageSaleImagesDirectory"].ToString();

                var storageDir = dir + Path.DirectorySeparatorChar + fileName;

                if (!System.IO.File.Exists(fileName))
                {
                    garageSalePicUpload.SaveAs(dir + Path.DirectorySeparatorChar + fileName);
                }
            }
            else
            {
                if (model.GarageSale.GargeSalePicLink == string.Empty)
                {
                    // Use the default pic.
                    model.GarageSale.GargeSalePicLink = "Insulators-3080-Karen-St-DBQ.jpg";
                }
            }

            // Set the sale dates in the web.config file and then set them as the model properties...
            model.GarageSale.DatesTimes.SaleDateOne = Convert.ToDateTime(ConfigurationManager.AppSettings["SaleDateOne"]);
            model.GarageSale.DatesTimes.SaleDateTwo = Convert.ToDateTime(ConfigurationManager.AppSettings["SaleDateTwo"]);
            model.GarageSale.DatesTimes.SaleDateThree = Convert.ToDateTime(ConfigurationManager.AppSettings["SaleDateThree"]);
            model.GarageSale.DatesTimes.SaleDateFour = Convert.ToDateTime(ConfigurationManager.AppSettings["SaleDateFour"]);
            model.GarageSale.ModifyDate = DateTime.Now;

            if (Session["UserSession"] != null)
            {
                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

                var userSession = Session["UserSession"] as UserSession;
                model.User = userSession.User;
                model.GarageSale.ModifyUser = model.User.UserName;
            }

            var errors = ModelState.Where(v => v.Value.Errors.Any());
            var repository = new GarageSaleRepository();

            if (ModelState.IsValid)
            {
                var saveResult = new UserSaveResult();

                saveResult = repository.UpdateGarageSale(model.GarageSale);

                model.GarageSale.GarageSaleId = saveResult.SaveResultId;

                ViewBag.NavViewSales = "active";

                if (saveResult.IsSaveSuccessful)
                {
                    Session["SaveSuccessful"] = true;
                }

                return RedirectToAction("EditGarageSale", new RouteValueDictionary(new
                {
                    controller = "Admin",
                    action = "EditGarageSale",
                    id = model.GarageSale.GarageSaleId
                }));
            }
            else
            {
                // Get the subcategories
                model.ItemCategories = repository.GetCategoriesAndSubcategories();
                return View("~/Views/Admin/GarageSales.cshtml", model);
            }
        }

        /// <summary>
        /// This edits a special item.
        /// </summary>
        /// <param name="special_item_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditSpecialItem(int special_item_id)
        {
            var userSession = new UserSession();

            if (Session["UserSession"] != null)
            {
                userSession = Session["UserSession"] as UserSession;

                var viewModel = new AdminViewModel();

                if (Session["AdminViewModel"] != null)
                {
                    viewModel = Session["AdminViewModel"] as AdminViewModel;
                }

                // Show the success message if the save worked.
                if (Session["SaveSuccessful"] != null)
                {
                    var saveSuccessful = Convert.ToBoolean(Session["SaveSuccessful"]);

                    if (saveSuccessful)
                    {
                        ViewBag.Invisible = "false";
                    }
                }

                var item = viewModel.GarageSaleSpecialItems.Where(m => m.SpecialItemsId == special_item_id).ToList();
                viewModel.GarageSaleSpecialItem = item[0];

                Session["AdminViewModel"] = viewModel;

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        /// <summary>
        /// This updates a special item.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="formCollection"></param>
        /// <param name="picUpload"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateSpecialItem(AdminViewModel model, FormCollection formCollection, HttpPostedFileBase picUpload)
        {
            var userSession = new UserSession();

            if (Session["UserSession"] != null)
            {
                userSession = Session["UserSession"] as UserSession;

                var viewModel = new AdminViewModel();

                if (Session["AdminViewModel"] != null)
                {
                    viewModel = Session["AdminViewModel"] as AdminViewModel;
                    model.GarageSaleSpecialItem.SaleId = viewModel.GarageSaleSpecialItem.SaleId;

                    viewModel.GarageSaleSpecialItem = model.GarageSaleSpecialItem;
                }

                model.GarageSaleSpecialItem.ItemSubcategoryId = Convert.ToInt32(formCollection["ddlCategories"]);

                if (picUpload != null)
                {
                    // Save the image file.
                    model.GarageSaleSpecialItem.PictureLink = picUpload.FileName;

                    var fileName = Path.GetFileName(model.GarageSaleSpecialItem.PictureLink);
                    var dir = ConfigurationManager.AppSettings["GarageSaleImagesDirectory"].ToString();

                    var storageDir = dir + Path.DirectorySeparatorChar + fileName;

                    if (!System.IO.File.Exists(fileName))
                    {
                        picUpload.SaveAs(dir + Path.DirectorySeparatorChar + fileName);
                    }
                }
                else
                {
                    // Don't do anything. Use the previously-uploaded pic.
                    //model.GarageSaleSpecialItem.PictureLink = "keep-calm-and-come-to-the-dupaco-garage-sale.png";
                }

                var repository = new GarageSaleRepository();
                var saveSuccessful = repository.UpdateGarageSaleSpecialItem(model.GarageSaleSpecialItem);

                if (saveSuccessful)
                {
                    Session["SaveSuccessful"] = true;

                    // Remove the updated item.
                    foreach (var item in viewModel.GarageSaleSpecialItems.ToList())
                    {
                        if (item.SpecialItemsId == model.GarageSaleSpecialItem.SpecialItemsId)
                        {
                            viewModel.GarageSaleSpecialItems.Remove(item);
                            viewModel.GarageSaleSpecialItems.Add(model.GarageSaleSpecialItem);
                        }
                    }
                }

                Session["AdminViewModel"] = viewModel;

                return RedirectToAction("EditSpecialItem", new RouteValueDictionary(new
                {
                    controller = "Admin",
                    action = "EditSpecialItem",
                    special_item_id = viewModel.GarageSaleSpecialItem.SpecialItemsId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        /// <summary>
        /// This deletes a special item.
        /// </summary>
        /// <param name="special_item_id"></param>
        /// <returns></returns>
        public ActionResult DeleteSpecialItem(int special_item_id)
        {
            if (Session["UserSession"] != null)
            {
                var saveSuccessful = false;

                var repository = new GarageSaleRepository();
                saveSuccessful = repository.DeleteGarageSpecialItems(special_item_id);

                TempData["ItemDeleteSuccessful"] = saveSuccessful;

                var viewModel = new AdminViewModel();

                if (Session["AdminViewModel"] != null)
                {
                    viewModel = Session["AdminViewModel"] as AdminViewModel;
                }

                return RedirectToAction("EditGarageSale", new RouteValueDictionary(new
                {
                    controller = "Admin",
                    action = "EditGarageSale",
                    id = viewModel.GarageSale.GarageSaleId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        #endregion        

        #region Blog posts

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
                Session["AdminViewModel"] = viewModel;
                ViewData["ShowBlogPost"] = "true";

                ViewBag.BlogPostsActive = "active";
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

                if (Session["AdminViewModel"] != null)
                {
                    viewModel = Session["AdminViewModel"] as AdminViewModel;

                    // Get the blog post.
                    var blogRepo = new AdminRepository();
                    viewModel.BlogPost = blogRepo.GetBlogPost(blog_post_id);
                    Session["AdminViewModel"] = viewModel;
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

        /// <summary>
        /// This deletes a garage sale blog post.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteBlogPost(int id)
        {
            if (Session["UserSession"] != null)
            {
                var viewModel = new AdminViewModel();

                if (Session["AdminViewModel"] != null)
                {
                    viewModel = Session["AdminViewModel"] as AdminViewModel;
                }

                var repository = new BlogPostRepository();
                var saveSuccessful = repository.DeleteBlogPost(id);

                TempData["ItemDeleteSuccessful"] = saveSuccessful;

                return RedirectToAction("EditGarageSale", new RouteValueDictionary(new
                {
                    controller = "Admin",
                    action = "EditGarageSale",
                    id = viewModel.GarageSale.GarageSaleId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        #endregion

        #region Headline news
        /// <summary>
        /// This loads the headline news page.
        /// </summary>
        /// <returns></returns>
        public ActionResult HeadlineNews()
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();
                viewModel.AdminUser = session.User;

                var repository = new AdminRepository();
                viewModel.AdminMessages = repository.GetAdminMessages();

                // Show the success message if the save worked.
                if (Session["PublishHeadlineNews"] != null)
                {
                    var saveSuccessful = Convert.ToBoolean(Session["PublishHeadlineNews"]);

                    if (saveSuccessful)
                    {
                        ViewBag.Invisible = "false";
                    }
                }

                ViewBag.HeadlinesActive = "active";
                ViewBag.NavAdmin = "active";
                return View(viewModel);
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        /// <summary>
        /// This saves a new headline news message.
        /// </summary>
        /// <param name="headlineNews"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PublishHeadlineNews(string headlineNews)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var saveResult = new UserSaveResult();
                var repository = new AdminRepository();

                var message = new AdminMessage
                {
                    MessageText = headlineNews,
                    MessageCreateDate = DateTime.Now,
                    MessagePublishDate = DateTime.Now,
                    MessageType = "headline"
                };

                saveResult = repository.SaveAdminMessage(message);

                if (saveResult.IsSaveSuccessful)
                {
                    Session["PublishHeadlineNews"] = true;
                }

                return RedirectToAction("HeadlineNews", new RouteValueDictionary(new
                {
                    controller = "Admin",
                    action = "HeadlineNews",
                    userId = session.User.UserId
                }));

            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }
        #endregion

        #region Publish garage sale instructions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="saleInstructions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PublishCreateASaleInstructions(string headlineNews)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var saveResult = new UserSaveResult();
                var repository = new AdminRepository();

                var message = new AdminMessage
                {
                    MessageText = headlineNews,
                    MessageCreateDate = DateTime.Now,
                    MessagePublishDate = DateTime.Now,
                    MessageType = "sale instructions"
                };

                saveResult = repository.SaveAdminMessage(message);

                if (saveResult.IsSaveSuccessful)
                {
                    Session["PublishHeadlineNews"] = true;
                }

                return RedirectToAction("HeadlineNews", new RouteValueDictionary(new
                {
                    controller = "Admin",
                    action = "HeadlineNews",
                    userId = session.User.UserId
                }));
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saleInstructions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PublishAdvancedSaleInstructions(string headlineNews)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var saveResult = new UserSaveResult();
                var repository = new AdminRepository();

                var message = new AdminMessage
                {
                    MessageText = headlineNews,
                    MessageCreateDate = DateTime.Now,
                    MessagePublishDate = DateTime.Now,
                    MessageType = "advanced sale instructions"
                };

                saveResult = repository.SaveAdminMessage(message);

                if (saveResult.IsSaveSuccessful)
                {
                    Session["PublishHeadlineNews"] = true;
                }

                return RedirectToAction("HeadlineNews", new RouteValueDictionary(new
                {
                    controller = "Admin",
                    action = "HeadlineNews",
                    userId = session.User.UserId
                }));
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }
        #endregion

        #region Message center
        /// <summary>
        /// This loads the message center page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MessageCenter()
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new AdminViewModel();
                viewModel.AdminUser = session.User;

                var repository = new AdminRepository();
                viewModel.ContactUsMessages = repository.GetContactUsMessages();

                if (Session["ReplySuccessful"] != null)
                {
                    if (Convert.ToBoolean(Session["ReplySuccessful"]))
                    {
                        ViewBag.ReplySuccessful = "visible";
                    }
                }

                var communityList = repository.GetCommunities();
                ViewData["CommunityList"] = new SelectList(communityList, "name", "name");

                ViewBag.MessageCenterActive = "active"; 
                return View(viewModel);
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        /// <summary>
        /// This sends a message from the user pages to the admins.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendContactUsMessage(FormCollection form)
        {
            if (form != null)
            {
                var contactName = form["txtContactName"].ToString();
                var contactEmail = form["txtContactEmail"].ToString();
                var contactPhone = form["txtContactPhone"].ToString();
                var contactMessage = form["txtContactUsMessage"].ToString();

                var message = new ContactUsMessage
                {
                    ContactName = contactName,
                    ContactEmail = contactEmail,
                    ContactPhone = contactPhone,
                    MessageText = contactMessage,
                    MessageSentDateTime = DateTime.Now
                };

                var repository = new AdminRepository();
                var saveResult = repository.SaveContactUsMessage(message);

                if (saveResult.IsSaveSuccessful)
                {
                    try
                    {
                        // Send contact us email.
                        var mailMessage = new System.Net.Mail.MailMessage(message.ContactEmail, ConfigurationManager.AppSettings["MarketingEmailAddress"].ToString());
                        mailMessage.IsBodyHtml = true;
                        mailMessage.Subject = "Dupaco Garage Sale User Message from " + message.ContactName;
                        mailMessage.Body = message.MessageText;
                        mailMessage.Priority = System.Net.Mail.MailPriority.Normal;

                        var smtp = new SmtpClient();
                        smtp.Host = ConfigurationManager.AppSettings["MailServer"].ToString();
                        smtp.Send(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(ex.ToString());
                    }

                    ViewBag.Message = "Message sent! Someone will get back to you soon!";
                }
                else
                {
                    ViewBag.Message = "There was a problem sending your message.  We are aware of the problem.  Please try again later.";
                }


            }

            return View("~/Views/Admin/MessageSent.cshtml");
        }

        /// <summary>
        /// This provides a mechanism for admins to reply to users messages.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReplyToContactUsMessage(FormCollection form)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                if (form != null)
                {
                    //var messageId = Convert.ToInt32(form["hdnMessageId"]);
                    var messageId = form["hdnMessageId"];
                    string messageText = form["txtReplyMessage"].ToString();

                    // Get the message by id.
                    var repository = new AdminRepository();
                    var contactUsMessage = repository.GetContactUsMessageById(Convert.ToInt32(messageId));

                    var reply = new MessageReply
                    {
                        MessageId = Convert.ToInt32(messageId),
                        ReplyText = messageText,
                        ReplyFrom = session.User.FirstName + " " + session.User.LastName,
                        ReplyTo = contactUsMessage.ContactEmail,
                        ReplyDateTime = DateTime.Now
                    };
                    
                    var saveResult = repository.SaveContactUsReplies(reply);

                    if (saveResult.IsSaveSuccessful)
                    {
                        try
                        {
                            // Reply to contact us email.
                            var mailMessage = new System.Net.Mail.MailMessage(ConfigurationManager.AppSettings["MarketingEmailAddress"].ToString(), reply.ReplyTo);
                            mailMessage.IsBodyHtml = true;
                            mailMessage.Subject = "Reply from the Dupaco Garage Sale Staff";
                            mailMessage.Body = reply.ReplyText;
                            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;

                            var smtp = new SmtpClient();
                            smtp.Host = ConfigurationManager.AppSettings["MailServer"].ToString();
                            smtp.Send(mailMessage);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log.Error(ex.ToString());
                        }

                        Session["ReplySuccessful"] = true;
                    }
                }

                return RedirectToAction("MessageCenter");
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        [HttpGet]
        public JsonResult GetEmailAddresses(string q)
        {
            var repository = new AdminRepository();
            var emailAddressList = repository.GetEmailAddressesByCriteria(q);

            var jsonResult = emailAddressList.Select(results => new { id = results.Email, name = results.Email });

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This allows for sending messages to individuals and to communities
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendMessage(FormCollection form)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var addresses = form["txtMessageTo"].ToString();
                var community = form["ddlCommunity"].ToString();
                var subject = form["txtMessageSubject"].ToString();
                var message = form["txtMessage"].ToString();

                // Send email to either individuals or to a community.
                if (addresses != string.Empty)
                {
                    // Send email to individuals.
                    var emailMessage = new EmailMessage
                    {
                        MessageFrom = session.User.Email,
                        MessageTo = addresses,
                        MessageText = message,
                        MessageSentDate = DateTimeOffset.Now
                    };

                    try
                    {
                        var mailMessage = new System.Net.Mail.MailMessage(ConfigurationManager.AppSettings["MarketingEmailAddress"].ToString(), emailMessage.MessageTo);
                        mailMessage.IsBodyHtml = true;
                        mailMessage.Subject = subject;
                        mailMessage.Body = emailMessage.MessageText;
                        mailMessage.Priority = System.Net.Mail.MailPriority.Normal;

                        var smtp = new SmtpClient();
                        smtp.Host = ConfigurationManager.AppSettings["MailServer"].ToString();
                        smtp.Send(mailMessage);

                        TempData["EmailSent"] = "true";
                    }
                    catch (Exception ex)
                    {
                        TempData["EmailSent"] = "false";
                        Logger.Log.Error(ex.ToString());
                    }

                }
                else if (community != string.Empty)
                {
                    // Send email to a community.
                    var repository = new AdminRepository();
                    var communityEmails = repository.GetEmailAddressesByCommunity(community);

                    foreach (var email in communityEmails)
                    {
                        var emailMessage = new EmailMessage
                        {
                            MessageFrom = session.User.Email,
                            MessageTo = email.Email,
                            MessageText = message,
                            MessageSentDate = DateTimeOffset.Now
                        };

                        try
                        {
                            var mailMessage = new System.Net.Mail.MailMessage(ConfigurationManager.AppSettings["MarketingEmailAddress"].ToString(), emailMessage.MessageTo);
                            mailMessage.IsBodyHtml = true;
                            mailMessage.Subject = subject;
                            mailMessage.Body = emailMessage.MessageText;
                            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;

                            var smtp = new SmtpClient();
                            smtp.Host = ConfigurationManager.AppSettings["MailServer"].ToString();
                            smtp.Send(mailMessage);

                            TempData["EmailSent"] = "true";
                        }
                        catch (Exception ex)
                        {
                            TempData["EmailSent"] = "false";
                            Logger.Log.Error(ex.ToString());
                        }
                    }
                }

                ViewBag.MessageCenterActive = "active";
                return RedirectToAction("MessageCenter");
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        #endregion
    }
}