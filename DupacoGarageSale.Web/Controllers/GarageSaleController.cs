using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Repository;
using DupacoGarageSale.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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

                //if (id == null)
                //{
                //    // Load an empty form for new users.
                //}
                //else
                //{
                //    // Load a saved garage sale by id.
                //    viewModel.Sale = repository.GetGarageSaleById((int)id);
                //}

                ViewBag.NavAddSale = "active";
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("SignIn", "Accounts");
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

            var testy = categoryIdList;
            var testy2 = model.Sale.GarageSaleItems;

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
        public ActionResult Update(GarageSaleViewModel model, FormCollection form)
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
                
                ViewBag.NavViewSales = "active";

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("SignIn", "Accounts");
            }  
        }

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
                ViewBag.NavViewSales = "active";

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

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("SignIn", "Accounts");
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateItinerary()
        {
            return View();
        }
    }
}