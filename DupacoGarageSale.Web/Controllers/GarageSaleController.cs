﻿using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Repository;
using DupacoGarageSale.Web.Helpers;
using DupacoGarageSale.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DupacoGarageSale.Web.Controllers
{
    public class GarageSaleController : Controller
    {
        #region Main garage sale actions

        /// <summary>
        /// This is the landing page for adding garage sales.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add(int? id)
        {
            var userSession = new UserSession();

            if (Session["UserSession"] != null)
            {
                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

                userSession = Session["UserSession"] as UserSession;

                var viewModel = new GarageSaleViewModel
                {
                    User = userSession.User,
                    Sale = new GarageSale()
                };

                var repository = new GarageSaleRepository();

                // Get the subcategories
                viewModel.ItemCategories = repository.GetCategoriesAndSubcategories();

                // Set the default pic
                if (viewModel.Sale.GargeSalePicLink == null)
                {
                    viewModel.Sale.GargeSalePicLink = "Insulators-3080-Karen-St-DBQ.jpg";
                }

                ViewBag.NavGarageSales = "active";
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }            
        }

        /// <summary>
        /// This saves a user's garage sale.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(GarageSaleViewModel model, FormCollection form)
        {
            var categoryIdList = new List<int>();
            model.Sale.GarageSaleItems = new List<GarageSaleItem>();            

            foreach (var key in form.AllKeys)
            {
                int categoryId;

                if (Int32.TryParse(key, out categoryId))
                {
                    var garageSaleItem = new GarageSaleItem
                    {
                        SaleId = model.Sale.GarageSaleId,
                        ItemSubcategoryId = categoryId
                    };

                    model.Sale.GarageSaleItems.Add(garageSaleItem);
                    categoryIdList.Add(categoryId);
                }
            }

            // Set the sale dates in the web.config file and then set them as the model properties...
            model.Sale.DatesTimes.SaleDateOne = Convert.ToDateTime(ConfigurationManager.AppSettings["SaleDateOne"]);
            model.Sale.DatesTimes.SaleDateTwo = Convert.ToDateTime(ConfigurationManager.AppSettings["SaleDateTwo"]);
            model.Sale.DatesTimes.SaleDateThree = Convert.ToDateTime(ConfigurationManager.AppSettings["SaleDateThree"]);
            model.Sale.DatesTimes.SaleDateFour = Convert.ToDateTime(ConfigurationManager.AppSettings["SaleDateFour"]);
            model.Sale.CreateDate = DateTime.Now;            

            if (Session["UserSession"] != null)
            {
                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

                var userSession = Session["UserSession"] as UserSession;
                model.User = userSession.User;
                model.Sale.ModifyUser = model.User.UserName;
            }

            var errors = ModelState.Where(v => v.Value.Errors.Any());
            var repository = new GarageSaleRepository();

            if (ModelState.IsValid)
            {
                var saveResult = new UserSaveResult();

                saveResult = repository.SaveGarageSale(model.Sale);

                model.Sale.GarageSaleId = saveResult.SaveResultId;

                ViewBag.NavAddSale = "active";   

                return RedirectToAction("View", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "View"
                }));
            }
            else
            {
                // Get the subcategories
                model.ItemCategories = repository.GetCategoriesAndSubcategories();
                return View("~/Views/GarageSale/Add.cshtml", model);
            }                     
        }

        /// <summary>
        /// This updates a user's garage sale.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(HttpPostedFileBase garageSalePicUpload, GarageSaleViewModel model, FormCollection form)
        {
            var categoryIdList = new List<int>();
            model.Sale.GarageSaleItems = new List<GarageSaleItem>();

            if (garageSalePicUpload != null)
            {
                // Save the image file.
                model.Sale.GargeSalePicLink = garageSalePicUpload.FileName;

                var fileName = Path.GetFileName(model.Sale.GargeSalePicLink);
                var dir = ConfigurationManager.AppSettings["GarageSaleImagesDirectory"].ToString();

                var storageDir = dir + Path.DirectorySeparatorChar + fileName;

                if (!System.IO.File.Exists(fileName))
                {
                    garageSalePicUpload.SaveAs(dir + Path.DirectorySeparatorChar + fileName);
                }
            }
            else
            {
                // Use the default pic.
                model.Sale.GargeSalePicLink = "Insulators-3080-Karen-St-DBQ.jpg";
            }

            foreach (var key in form.AllKeys)
            {
                int categoryId;

                if (Int32.TryParse(key, out categoryId))
                {
                    var garageSaleItem = new GarageSaleItem
                    {
                        SaleId = model.Sale.GarageSaleId,
                        ItemSubcategoryId = categoryId
                    };

                    model.Sale.GarageSaleItems.Add(garageSaleItem);
                    categoryIdList.Add(categoryId);
                }
            }

            var testy = categoryIdList;
            var testy2 = model.Sale.GarageSaleItems;

            // Set the sale dates in the web.config file and then set them as the model properties...
            model.Sale.DatesTimes.SaleDateOne = Convert.ToDateTime(ConfigurationManager.AppSettings["SaleDateOne"]);
            model.Sale.DatesTimes.SaleDateTwo = Convert.ToDateTime(ConfigurationManager.AppSettings["SaleDateTwo"]);
            model.Sale.DatesTimes.SaleDateThree = Convert.ToDateTime(ConfigurationManager.AppSettings["SaleDateThree"]);
            model.Sale.DatesTimes.SaleDateFour = Convert.ToDateTime(ConfigurationManager.AppSettings["SaleDateFour"]);
            model.Sale.ModifyDate = DateTime.Now;

            if (Session["UserSession"] != null)
            {
                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

                var userSession = Session["UserSession"] as UserSession;
                model.User = userSession.User;
                model.Sale.ModifyUser = model.User.UserName;
            }

            var errors = ModelState.Where(v => v.Value.Errors.Any());
            var repository = new GarageSaleRepository();

            if (ModelState.IsValid)
            {
                var saveResult = new UserSaveResult();

                saveResult = repository.UpdateGarageSale(model.Sale);

                model.Sale.GarageSaleId = saveResult.SaveResultId;

                ViewBag.NavViewSales = "active";

                if (saveResult.IsSaveSuccessful)
                {
                    Session["SaveSuccessful"] = true;
                }

                return RedirectToAction("Edit", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "Edit",
                    id = model.Sale.GarageSaleId
                }));
            }
            else
            {
                // Get the subcategories
                model.ItemCategories = repository.GetCategoriesAndSubcategories();
                return View("~/Views/GarageSale/Edit.cshtml", model);
            }
        }

        /// <summary>
        /// This display's the user's garge sale(s).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult View()
        {
            var userSession = new UserSession();

            if (Session["UserSession"] != null)
            {
                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();

                userSession = Session["UserSession"] as UserSession;

                var repository = new GarageSaleRepository();

                // Load saved garage sales by user name.
                var viewModel = new GarageSaleViewModel
                {
                    User = userSession.User,
                    Sale = new GarageSale()                    
                };

                viewModel.GarageSales = repository.GetGarageSaleByUserName(viewModel.User.UserName);
                ViewBag.NavGarageSales = "active";

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }  
        }

        /// <summary>
        /// Edit a garage sale.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
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

                var viewModel = new GarageSaleViewModel
                {
                    User = userSession.User,
                    Sale = repository.GetGarageSaleAndItemsById(id),
                    SelectedCategories = new List<int>()
                };

                foreach (var itemId in viewModel.Sale.GarageSaleItems)
                {
                    viewModel.SelectedCategories.Add(itemId.ItemSubcategoryId);
                }

                // Get the categories and subcategories.
                viewModel.ItemCategories = repository.GetCategoriesAndSubcategories();

                // Get the special items.
                viewModel.GarageSaleSpecialItems = repository.GetGarageSaleSpecialItems(viewModel.Sale.GarageSaleId);
                foreach (var itemId in viewModel.GarageSaleSpecialItems)
                {
                    viewModel.SelectedCategories.Add(itemId.ItemSubcategoryId);
                }

                var selectedCategories = viewModel.SelectedCategories.ToArray();
                ViewBag.SelectedCategories = string.Join(",", selectedCategories);

                // Get the blog posts.
                var blogRepo = new BlogPostRepository();
                viewModel.BlogPosts = blogRepo.GetBlogPosts(viewModel.Sale.GarageSaleId);

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

                // Save the viewmodel for later use.
                Session["ViewModel"] = viewModel;

                // Clear the session object.
                Session["SaveSuccessful"] = null;

                ViewBag.NavGarageSales = "active";
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        /// <summary>
        /// Delete a garage sale.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["UserSession"] != null)
            {
                var saveSuccessful = false;

                var repository = new GarageSaleRepository();
                saveSuccessful = repository.DeleteGarageSale(id);

                TempData["SaveSuccessful"] = saveSuccessful;

                return RedirectToAction("View", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "View",
                    id = id
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        #endregion

        #region Garage sale special items

        /// <summary>
        /// This adds a special item.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddSpecialItem()
        {
            var userSession = new UserSession();

            if (Session["UserSession"] != null)
            {
                userSession = Session["UserSession"] as UserSession;

                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                }

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
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

                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
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

                Session["ViewModel"] = viewModel;

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
        public ActionResult UpdateSpecialItem(GarageSaleViewModel model, FormCollection formCollection, HttpPostedFileBase picUpload)
        {
            var userSession = new UserSession();

            if (Session["UserSession"] != null)
            {
                userSession = Session["UserSession"] as UserSession;

                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
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

                Session["ViewModel"] = viewModel;

                return RedirectToAction("EditSpecialItem", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
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
        /// This saves a special item.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="formCollection"></param>
        /// <param name="picUpload"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveSpecialItem(GarageSaleViewModel model, FormCollection formCollection, HttpPostedFileBase picUpload)
        {
            var viewModel = new GarageSaleViewModel();

            if (Session["ViewModel"] != null)
            {
                viewModel = Session["ViewModel"] as GarageSaleViewModel;

                model.GarageSaleSpecialItem.ItemSubcategoryId = Convert.ToInt32(formCollection["ddlCategories"]);
                model.GarageSaleSpecialItem.SaleId = viewModel.Sale.GarageSaleId;
                //viewModel.Sale.ModifyDate = DateTime.Now;
                //viewModel.Sale.ModifyUser = viewModel.User.UserName;

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
                    // Use the default pic.
                    model.GarageSaleSpecialItem.PictureLink = "keep-calm-and-come-to-the-dupaco-garage-sale.png";
                }

                var userSession = new UserSession();

                if (Session["UserSession"] != null)
                {
                    userSession = Session["UserSession"] as UserSession;
                }

                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");                

                var repository = new GarageSaleRepository();
                var saveResult = new UserSaveResult();

                //repository.SaveGarageSale(viewModel.Sale);
                saveResult = repository.SaveGarageSaleSpecialItem(model.GarageSaleSpecialItem);

                if (saveResult.IsSaveSuccessful)
                {
                    model.GarageSaleSpecialItem.SpecialItemsId = saveResult.SaveResultId;
                    Session["SaveSuccessful"] = true;
                }
                else
                {
                    // Indicate in some way that the save failed.
                }

                viewModel.GarageSaleSpecialItem = model.GarageSaleSpecialItem;

                Session["ViewModel"] = viewModel;
                Session["UserSession"] = userSession;
                ViewBag.NavViewSales = "active";

                return RedirectToAction("Edit", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "Edit",
                    id = viewModel.Sale.GarageSaleId
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

                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                }

                return RedirectToAction("Edit", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "Edit",
                    id = viewModel.Sale.GarageSaleId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        #endregion

        #region View garage sale

        /// <summary>
        /// This displays a garage sale.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewGarageSale(int id)
        {
            // Load the states dropdown.
            var addressRepository = new AddressRepository();
            var statesList = addressRepository.GetStates();
            ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

            var repository = new GarageSaleRepository();

            var viewModel = new GarageSaleViewModel
            {
                Sale = repository.GetGarageSaleAndItemsById(id),
                SelectedCategories = new List<int>()
            };

            // Get the user session
            UserSession session = null;
            if (Session["UserSession"] != null)
            {
                session = Session["UserSession"] as UserSession;
                viewModel.User = session.User;

                // Check to see if the current user has fave'd this garage sale.
                var faveGarageSale = repository.CheckForFavedGarageSale(id, viewModel.User.UserId);
                if (faveGarageSale.FavoriteId != 0)
                {
                    viewModel.FaveGarageSales = new List<FavoriteGarageSale>();
                    viewModel.FaveGarageSales.Add(faveGarageSale);
                }
            }

            foreach (var itemId in viewModel.Sale.GarageSaleItems)
            {
                viewModel.SelectedCategories.Add(itemId.ItemSubcategoryId);
            }

            // Get the categories and subcategories.
            viewModel.ItemCategories = repository.GetCategoriesAndSubcategories();

            var selectedCategories = viewModel.SelectedCategories.ToArray();
            ViewBag.SelectedCategories = string.Join(",", selectedCategories);

            // Get the special items.
            viewModel.GarageSaleSpecialItems = repository.GetGarageSaleSpecialItems(viewModel.Sale.GarageSaleId);

            // Get the blog posts.
            var blogRepo = new BlogPostRepository();
            viewModel.BlogPosts = blogRepo.GetBlogPosts(viewModel.Sale.GarageSaleId);

            // Get the messages.
            viewModel.GarageSaleMessages = repository.GetGarageSaleMessages(id);

            // Save the viewmodel for later use.
            Session["ViewModel"] = viewModel;

            ViewBag.NavGarageSales = "active";

            return View(viewModel);
        }

        #endregion

        #region View garage sale categories

        public ActionResult GetItemsByCategory(int categoryId)
        {
            var viewModel = new GarageSaleViewModel();

            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                viewModel.User = session.User;
            }

            var repository = new GarageSaleRepository();
            viewModel.SearchResults = repository.SearchGarageSalesByCategory(categoryId);

            return PartialView("_ItemCategories", viewModel);
        }

        #endregion

        #region Garage sale blog posts

        /// <summary>
        /// This saves a garage sale blog post
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveBlogPost(GarageSaleViewModel model, HttpPostedFileBase garageSalePicUpload)
        {
            if (Session["UserSession"] != null)
            {
                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                    viewModel.GarageSaleBlogPost = model.GarageSaleBlogPost;
                    viewModel.GarageSaleBlogPost.SaleId = viewModel.Sale.GarageSaleId;

                    if (garageSalePicUpload != null)
                    {
                        // Save the image file.
                        viewModel.GarageSaleBlogPost.ImageUri = garageSalePicUpload.FileName;

                        var fileName = Path.GetFileName(viewModel.GarageSaleBlogPost.ImageUri);
                        var dir = ConfigurationManager.AppSettings["GarageSaleImagesDirectory"].ToString();

                        var storageDir = dir + Path.DirectorySeparatorChar + fileName;

                        if (!System.IO.File.Exists(fileName))
                        {
                            garageSalePicUpload.SaveAs(dir + Path.DirectorySeparatorChar + fileName);
                        }
                    }
                }

                if (viewModel.GarageSaleBlogPost.MediaTypeId == 2)
                {
                    // Convert the 'watch' url to an 'embed' url.
                    // https://www.youtube.com/watch?v=3YdeSNcAWos
                    // https://www.youtube.com/embed/3YdeSNcAWos

                    var youTubeWatchUri = viewModel.GarageSaleBlogPost.YouTubeUri;

                    const string pattern = @"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)";
                    const string youTubeEmbedUri = "https://www.youtube.com/embed/$1";

                    var rgx = new Regex(pattern);
                    var result = rgx.Replace(youTubeWatchUri, youTubeEmbedUri);
                    viewModel.GarageSaleBlogPost.YouTubeUri = TextHelper.EncodeText(result);
                }
                else if (viewModel.GarageSaleBlogPost.MediaTypeId == 3)
                {
                    viewModel.GarageSaleBlogPost.VineUri = TextHelper.EncodeText(viewModel.GarageSaleBlogPost.VineUri);
                } 

                var repository = new BlogPostRepository();
                var saveResult = repository.SaveBlogPost(viewModel.GarageSaleBlogPost, viewModel.User.UserId);

                if (saveResult.IsSaveSuccessful)
                {
                    viewModel.GarageSaleBlogPost.BlogPostId = saveResult.SaveResultId;
                    Session["SaveSuccessful"] = true;
                }
                else
                {
                    // Indicate in some way that the save failed.
                }

                // Save the viewmodel for later use.
                Session["ViewModel"] = viewModel;

                return RedirectToAction("Edit", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "Edit",
                    id = viewModel.Sale.GarageSaleId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        public ActionResult DeleteBlogPost(int id)
        {
            if (Session["UserSession"] != null)
            {
                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                }

                var repository = new BlogPostRepository();
                var saveSuccessful = repository.DeleteBlogPost(id);

                TempData["ItemDeleteSuccessful"] = saveSuccessful;

                return RedirectToAction("Edit", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "Edit",
                    id = viewModel.Sale.GarageSaleId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        #endregion

        #region Garage sale messages

        [HttpPost]
        public ActionResult SendGarageSaleMessage(GarageSaleViewModel model)
        {
            if (Session["UserSession"] != null)
            {
                var userSession = Session["UserSession"] as UserSession;
                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                    viewModel.GarageSaleMessage = new GarageSaleMessage();
                    viewModel.GarageSaleMessage.MessageFrom = userSession.User.UserName;
                    viewModel.GarageSaleMessage.SaleId = viewModel.Sale.GarageSaleId;
                    viewModel.GarageSaleMessage.MessageSent = DateTime.Now;
                    viewModel.GarageSaleMessage.MessageText = model.GarageSaleMessage.MessageText;

                    var repository = new GarageSaleRepository();
                    var saveResult = repository.SaveGarageSaleMessage(viewModel.GarageSaleMessage);

                    if (saveResult.IsSaveSuccessful)
                    {
                        viewModel.GarageSaleMessage.MessageId = saveResult.SaveResultId;
                        Session["SaveSuccessful"] = true;
                    }
                    else
                    {
                        // Indicate in some way that the save failed.
                    }
                }

                return RedirectToAction("ViewGarageSale", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "ViewGarageSale",
                    id = viewModel.Sale.GarageSaleId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }            
        }

        [HttpPost]
        public ActionResult ReplyToGarageSaleMessage(GarageSaleViewModel model)
        {
            if (Session["UserSession"] != null)
            {
                var userSession = Session["UserSession"] as UserSession;
                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                    viewModel.GarageSaleMessage = new GarageSaleMessage();
                    viewModel.GarageSaleMessage.MessageFrom = userSession.User.UserName;
                    viewModel.GarageSaleMessage.SaleId = viewModel.Sale.GarageSaleId;
                    viewModel.GarageSaleMessage.MessageSent = DateTime.Now;
                    viewModel.GarageSaleMessage.MessageText = model.GarageSaleMessage.MessageText;

                    var repository = new GarageSaleRepository();
                    var saveResult = repository.SaveGarageSaleMessage(viewModel.GarageSaleMessage);

                    if (saveResult.IsSaveSuccessful)
                    {
                        viewModel.GarageSaleMessage.MessageId = saveResult.SaveResultId;
                        Session["SaveSuccessful"] = true;
                    }
                    else
                    {
                        // Indicate in some way that the save failed.
                    }
                }

                return RedirectToAction("Edit", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "Edit",
                    id = viewModel.Sale.GarageSaleId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        #endregion

        #region Search sales

        /// <summary>
        /// This performs a search by free-text criteria and/or selected subcategories.
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public ActionResult Search(FormCollection formCollection, string s)
        {
            var viewModel = new GarageSaleViewModel();

            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                viewModel.User = session.User;

                // Get the user's address.
                if (viewModel.User.Address != null)
                {
                    ViewBag.CenterAddress = viewModel.User.Address.Address1 + " " + viewModel.User.Address.Address2 + " " + viewModel.User.Address.City + " " +
                        viewModel.User.Address.State + " " + viewModel.User.Address.Zip;
                }
            }

            var repository = new GarageSaleRepository();

            if (formCollection.AllKeys.Count() != 0)
            {
                var searchCriteria = formCollection["txtSearch"].ToString();
                var itemSubcategory = Convert.ToInt32(formCollection["ddlCategories"]);
                var radius = formCollection["ddlRadius"].ToString();
                var address = formCollection["txtAddress"].ToString();

                ViewBag.SearchAddress = address;
                ViewBag.Radius = radius;

                //var repository = new GarageSaleRepository();
                viewModel.SearchResults = repository.SearchGarageSales(searchCriteria, itemSubcategory);

                var addresses = new List<string>();

                // Take all the items in the search results and piece together addresses.
                foreach (var item in viewModel.SearchResults.GarageSaleItems)
                {
                    addresses.Add(item.Address1 + ' ' + item.Address2 + ' ' + item.City + ' ' + item.State + ' ' + item.ZipCode);
                }                

                ViewBag.Addresses = addresses.ToArray();
            }
            else
            {
                // Show some random items as you land on the page for the first time.
                if (s != null)
                {
                    var randomSpecialItems = ItemsHelper.GetRandomSpecialItems();
                    var randomGarageSaleItems = ItemsHelper.GetRandomGarageSaleItems();
                    viewModel.SearchResults = new GarageSaleSearchResults
                    {
                        SpecialItems = repository.GetGarageSaleSpecialItems(randomSpecialItems),
                        GarageSaleItems = new List<GarageSaleSearchItem>()
                    };
                }
                else
                {
                    // Initialise a container of empty results.
                    viewModel.SearchResults = new GarageSaleSearchResults
                    {
                        GarageSaleItems = new List<GarageSaleSearchItem>(),
                        SpecialItems = new List<SpecialItem>()
                    };
                }
            }

            viewModel.ItemCategories = repository.GetCategoriesAndSubcategories();

            ViewBag.NavSearch = "active";

            return View(viewModel);
        }

        /// <summary>
        /// This performs a search based on free-text criteria.
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public ActionResult SearchByCriteria(FormCollection formCollection)
        {
            var viewModel = new GarageSaleViewModel();

            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                viewModel.User = session.User;

                // Get the user's address.
                if (viewModel.User.Address != null)
                {
                    ViewBag.CenterAddress = viewModel.User.Address.Address1 + " " + viewModel.User.Address.Address2 + " " + viewModel.User.Address.City + " " +
                        viewModel.User.Address.State + " " + viewModel.User.Address.Zip;
                }
            }

            var repository = new GarageSaleRepository();
            var searchCriteria = formCollection["txtSearchCriteria"].ToString();
            ViewBag.SearchCriteria = searchCriteria;
            ViewBag.SearchResults = searchCriteria;
            viewModel.SearchResults = repository.SearchGarageSales(searchCriteria);
            viewModel.ItemCategories = repository.GetCategoriesAndSubcategories();

            if (viewModel.SearchResults.SpecialItems.Count == 0 && viewModel.SearchResults.GarageSaleItems.Count == 0)
            {
                ViewBag.SearchResultsCount = 0;
            }

            viewModel.SelectedCategories = new List<int>();

            // Set the selected categories for the special items.
            if (viewModel.SearchResults.SpecialItems.Count > 0)
            {
                foreach (var item in viewModel.SearchResults.SpecialItems)
                {
                    viewModel.SelectedCategories.Add(item.ItemSubcategoryId);
                }
            }

            // Set the selected categories for the regular items.
            if (viewModel.SearchResults.GarageSaleItems.Count > 0)
            {
                foreach (var item in viewModel.SearchResults.GarageSaleItems)
                {
                    viewModel.SelectedCategories.Add(item.ItemSubcategoryId);
                }
            }

            var selectedCategories = viewModel.SelectedCategories.ToArray();
            ViewBag.SelectedCategories = string.Join(",", selectedCategories);

            Session["ViewModel"] = viewModel;
            ViewBag.NavSearch = "active";

            return View("Search", viewModel);
        }

        /// <summary>
        /// This searches the garage sales by category.
        /// </summary>
        /// <param name="itemSubcategoryId"></param>
        /// <returns></returns>
        public ActionResult SearchBySubcategory(int itemSubcategoryId)
        {
            var viewModel = new GarageSaleViewModel();

            var repository = new GarageSaleRepository();
            viewModel.SearchResults = repository.SearchGarageSalesBySubcategory(itemSubcategoryId);
            viewModel.ItemCategories = repository.GetCategoriesAndSubcategories();

            Session["ViewModel"] = viewModel;

            return View("SearchResults", viewModel);
        }

        #endregion

        #region Garage sale itineraries

        [HttpGet]
        public ActionResult ViewItinerary(int? id)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                // Get the user's itinerary by user id.
                var repository = new ItineraryRepository();
                var itineraryList = new List<GarageSaleItinerary>();

                if (id != null)
                {
                    itineraryList = repository.GetItineraryByUserId(Convert.ToInt32(id));
                }
                else
                {
                    itineraryList = repository.GetItineraryByUserId(session.User.UserId);
                }

                // Add it to the viewModel.
                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                }

                viewModel.User = session.User;
                viewModel.GarageSaleItinerary = itineraryList;

                if (Session["ItineraryLegDeleted"] != null)
                {
                    var deleteSuccessful = Convert.ToBoolean(Session["ItineraryLegDeleted"]);

                    if (deleteSuccessful)
                    {
                        ViewBag.Invisible = "false";
                    }                    
                }

                // Cleanup.
                Session["ItineraryLegDeleted"] = null;

                Session["ViewModel"] = viewModel;
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        /// <summary>
        /// This adds a new, blank itinerary leg.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddToItinerary(int userId)
        {
            var viewModel = new GarageSaleViewModel();

            if (Session["ViewModel"] != null)
            {
                viewModel = Session["ViewModel"] as GarageSaleViewModel;
            }

            //Adding an itinerary here means it is not tied to any particular sale
            var saleId = 22; // This is the null sale id.

            // Check to see if the user has an itinerary. If not, create a new one. If so, update the existing one.
            var repository = new ItineraryRepository();

            var itinerary = repository.CheckForItinerary(userId);

            if (itinerary.ItineraryId != 0)
            {
                // Add a new leg to the existing itinerary.
                var saveResult = repository.SaveItineraryLeg(itinerary.ItineraryId, saleId);
            }
            else
            {
                // Create a new itinerary.
                itinerary = new Itinerary
                {
                    SaleId = saleId, 
                    ItineraryCreateDate = DateTime.Now,
                    ItineraryModifyDate = DateTime.Now,
                    ItineraryOwner = userId
                };

                var saveResult = repository.SaveItinerary(itinerary);

                if (saveResult.IsSaveSuccessful)
                {
                    itinerary.ItineraryId = saveResult.SaveResultId;
                    TempData["ItineraryId"] = itinerary.ItineraryId;
                    ViewBag.ItineraryId = itinerary.ItineraryId;
                    viewModel.UserItinerary = new Itinerary();
                    viewModel.UserItinerary = itinerary;
                }
            }

            Session["ViewModel"] = viewModel;

            return RedirectToAction("ViewItinerary", new RouteValueDictionary(new
            {
                id = viewModel.User.UserId
            }));
        }

        [HttpGet]
        public ActionResult AddToUserItinerary(int saleId, int userId)
        {
            var viewModel = new GarageSaleViewModel();

            if (Session["ViewModel"] != null)
            {
                viewModel = Session["ViewModel"] as GarageSaleViewModel;
            }

            // Check to see if the user has an itinerary. If not, create a new one. If so, update the existing one.
            var repository = new ItineraryRepository();

            var itinerary = repository.CheckForItinerary(userId);

            if (itinerary.ItineraryId != 0)
            {
                // Add a new leg to the existing itinerary.
                var saveResult = repository.SaveItineraryLeg(itinerary.ItineraryId, saleId);
            }
            else
            {
                // Create a new itinerary.
                itinerary = new Itinerary
                {
                    SaleId = saleId,
                    ItineraryCreateDate = DateTime.Now,
                    ItineraryModifyDate = DateTime.Now,
                    ItineraryOwner = userId
                };

                var saveResult = repository.SaveItinerary(itinerary);

                if (saveResult.IsSaveSuccessful)
                {
                    itinerary.ItineraryId = saveResult.SaveResultId;
                    TempData["ItineraryId"] = itinerary.ItineraryId;
                    ViewBag.ItineraryId = itinerary.ItineraryId;
                    viewModel.UserItinerary = new Itinerary();
                    viewModel.UserItinerary = itinerary;
                }
            }

            Session["ViewModel"] = viewModel;

            return RedirectToAction("ViewItinerary", new RouteValueDictionary(new
            {
                id = userId
            }));
        }

        [HttpPost]
        public ActionResult AddToItinerary(int saleId, int userId)
        {
            var viewModel = new GarageSaleViewModel();

            if (Session["ViewModel"] != null)
            {
                viewModel = Session["ViewModel"] as GarageSaleViewModel;
            }

            // Check to see if the user has an itinerary. If not, create a new one. If so, update the existing one.
            var repository = new ItineraryRepository();

            var itinerary = repository.CheckForItinerary(userId);

            if (itinerary.ItineraryId != 0)
            {
                // Add a new leg to the existing itinerary.
                var saveResult = repository.SaveItineraryLeg(itinerary.ItineraryId, saleId);
            }
            else
            {
                // Create a new itinerary.
                itinerary = new Itinerary
                {
                    SaleId = saleId,
                    ItineraryCreateDate = DateTime.Now,
                    ItineraryModifyDate = DateTime.Now,
                    ItineraryOwner = userId
                };

                var saveResult = repository.SaveItinerary(itinerary);

                if (saveResult.IsSaveSuccessful)
                {
                    itinerary.ItineraryId = saveResult.SaveResultId;
                    TempData["ItineraryId"] = itinerary.ItineraryId;
                    ViewBag.ItineraryId = itinerary.ItineraryId;
                    viewModel.UserItinerary = new Itinerary();
                    viewModel.UserItinerary = itinerary;
                }
            }

            Session["ViewModel"] = viewModel;

            return RedirectToAction("ViewGarageSale", new RouteValueDictionary(new
            {
                id = saleId
            }));
        }

        /// <summary>
        /// This gets a single garage sale by map address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetGarageSaleByAddress(string address)
        {
            string[] searchAdddress = address.Split(' ');
            var garageSaleAddress = new GarageSaleAddress();
            garageSaleAddress.Address1 = searchAdddress[0] + " " + searchAdddress[1];

            var repository = new GarageSaleRepository();
            var garageSaleId = repository.GetGarageSaleIdByAddresses(garageSaleAddress.Address1);

            return RedirectToAction("ViewGarageSale", new RouteValueDictionary(new
            {
                controller = "GarageSale",
                action = "ViewGarageSale",
                id = garageSaleId
            }));
        }

        [HttpGet]
        public ActionResult DeleteItineraryLeg(int legId)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;

                    // Delete legs, only if it matches the current user.
                    var repository = new ItineraryRepository();
                    var saveResult = repository.DeleteItineraryLeg(legId, viewModel.GarageSaleItinerary[0].ItineraryId, viewModel.GarageSaleItinerary[0].SaleId,
                        session.User.UserId);

                    if (saveResult.IsSaveSuccessful)
                    {
                        Session["ItineraryLegDeleted"] = true;
                    }
                }

                return RedirectToAction("ViewItinerary", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "ViewItinerary"
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        /// <summary>
        /// This is the search page from the itinerary page.
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ItineraryPageSearch(FormCollection formCollection)
        {
            var viewModel = new GarageSaleViewModel();

            if (Session["ViewModel"] != null)
            {
                viewModel = Session["ViewModel"] as GarageSaleViewModel;
            }

            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                viewModel.User = session.User;

                var repository = new GarageSaleRepository();
                var searchCriteria = formCollection["txtSearchCriteria"].ToString();
                viewModel.SearchResults = repository.SearchGarageSales(searchCriteria);
            }

            Session["ViewModel"] = viewModel;

            return RedirectToAction("ViewItinerary", viewModel);
        }

        #endregion

        #region Filter results

        [HttpPost]
        public ActionResult FilterResults(FormCollection form)
        {
            var searchCriteria = string.Empty;

            if (form["hdnSearchCriteria"] != null)
            {
                searchCriteria = form["hdnSearchCriteria"].ToString();
            }
            else
            {
                searchCriteria = form["hdnSearchResults"].ToString();
            }

            var radius = form["ddlRadius"].ToString();
            var address = form["txtAddress"].ToString();

            var categoryIdList = new List<int>();

            foreach (var key in form.AllKeys)
            {
                int categoryId;

                if (Int32.TryParse(key, out categoryId))
                {
                    categoryIdList.Add(categoryId);
                }
            }

            var viewModel = new GarageSaleViewModel();

            if (Session["ViewModel"] != null)
            {
                viewModel = Session["ViewModel"] as GarageSaleViewModel;
            }

            var repository = new GarageSaleRepository();
            viewModel.SearchResults = repository.SearchGarageSales(searchCriteria, categoryIdList);

            // if address != null compute the radius
            var addresses = new List<string>();

            // Take all the items in the search results and piece together addresses.
            foreach (var item in viewModel.SearchResults.GarageSaleItems)
            {
                addresses.Add(item.Address1 + ' ' + item.Address2 + ' ' + item.City + ' ' + item.State + ' ' + item.ZipCode);
                viewModel.SelectedCategories.Add(item.ItemSubcategoryId);
            }

            // Get the special items addresses
            if (viewModel.SearchResults.SpecialItems.Count > 0)
            {
                foreach (var item in viewModel.SearchResults.SpecialItems)
                {
                    var saleAddress = repository.GetGarageSaleAddressBySaleId(item.SaleId);
                    addresses.Add(saleAddress.Address1 + ' ' + saleAddress.Address2 + ' ' + saleAddress.City + ' ' + saleAddress.State + ' ' + saleAddress.ZipCode);
                    viewModel.SelectedCategories.Add(item.ItemSubcategoryId);
                }
            }

            var selectedCategories = viewModel.SelectedCategories.ToArray();
            ViewBag.SelectedCategories = string.Join(",", selectedCategories);

            if (addresses.Count > 0)
            {
                ViewBag.Addresses = addresses.ToArray();
                ViewBag.ShowMap = "true";
            }

            ViewBag.SearchAddress = address;
            ViewBag.Radius = radius;

            return View("Search", viewModel);
        }

        #endregion

        #region Add to faves

        public ActionResult AddSaleToFaves(int saleId, int userId)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                var repository = new GarageSaleRepository();
                var saveResult = repository.SaveFaveGarageSale(saleId, userId);

                return RedirectToAction("ViewGarageSale", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "ViewGarageSale",
                    id = saleId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        public ActionResult RemoveSaleFromFaves(int faveId, int saleId)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                var repository = new GarageSaleRepository();
                var saveResult = repository.RemoveFaveGarageSale(faveId);

                return RedirectToAction("ViewGarageSale", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "ViewGarageSale",
                    id = saleId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        #endregion
    }
}