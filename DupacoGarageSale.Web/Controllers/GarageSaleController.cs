using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Repository;
using DupacoGarageSale.Web.Helpers;
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
    public class GarageSaleController : Controller
    {
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

                var selectedCategories = viewModel.SelectedCategories.ToArray();
                ViewBag.SelectedCategories = string.Join(",", selectedCategories);

                // Get the special items.
                viewModel.GarageSaleSpecialItems = repository.GetGarageSaleSpecialItems(viewModel.Sale.GarageSaleId);

                // Get the blog posts.
                var blogRepo = new BlogPostRepository();
                viewModel.BlogPosts = blogRepo.GetBlogPosts(viewModel.Sale.GarageSaleId);

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

        #region Garage sale special items

        [HttpGet]
        public ActionResult AddSpecialItem()
        {
            var userSession = new UserSession();

            if (Session["UserSession"] != null)
            {
                userSession = Session["UserSession"] as UserSession;
            }

            var viewModel = new GarageSaleViewModel();

            if (Session["ViewModel"] != null)
            {
                viewModel = Session["ViewModel"] as GarageSaleViewModel;
            }

            return View(viewModel);
        }

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
                    viewModel.GarageSaleBlogPost.YouTubeUri = TextHelper.EncodeText(viewModel.GarageSaleBlogPost.YouTubeUri);
                }
                else if (viewModel.GarageSaleBlogPost.MediaTypeId == 3)
                {
                    viewModel.GarageSaleBlogPost.VineUri = TextHelper.EncodeText(viewModel.GarageSaleBlogPost.VineUri);
                } 

                var repository = new BlogPostRepository();
                var saveResult = repository.SaveBlogPost(viewModel.GarageSaleBlogPost);

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

        [HttpGet]
        public ActionResult CreateItinerary()
        {
            return View();
        }
    }
}