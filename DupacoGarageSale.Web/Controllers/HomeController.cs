﻿using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Repository;
using DupacoGarageSale.Data.Services;
using DupacoGarageSale.Web.Helpers;
using DupacoGarageSale.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DupacoGarageSale.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            UserSession session = null;
            var repository = new GarageSaleRepository();
            var viewModel = new GarageSaleViewModel();

            if (Session["ViewModel"] != null)
            {
                viewModel = Session["ViewModel"] as GarageSaleViewModel;
            }

            if (Session["SearchData"] != null)
            {
                var searchData = Session["SearchData"] as string[];
                ViewBag.Address = searchData[0].ToString();
                ViewBag.From = searchData[1].ToString();
                ViewBag.To = searchData[2].ToString();
                ViewBag.SearchCriteria = searchData[3].ToString();
                ViewBag.SearchRadius = searchData[5].ToString();
                ViewBag.Community = searchData[5].ToString();
            }

            if (Session["ShowModal"] != null)
            {
                ViewBag.ShowModal = "true";
                ViewBag.WaypointAddress = Session["WaypointAddress"].ToString();

                // Clean up.
                Session["ShowModal"] = null;
            }

            viewModel.SelectedCategories = new List<int>();
            viewModel.ItemCategories = repository.GetCategoriesAndSubcategories();

            if (viewModel.SearchResults == null)
            {
                // Get some random items for the home page.
                var randomSpecialItems = repository.GetRandomSpecialItems();
                viewModel.GarageSaleSpecialItems = randomSpecialItems;

                // Get random garage sale items for the home page.
                var randomGarageSaleItems = repository.GetRandomGarageSaleItems();
                viewModel.GarageSaleItems = randomGarageSaleItems;

                // Set the selected categories for the special items.
                if (viewModel.GarageSaleSpecialItems.Count > 0)
                {
                    foreach (var item in viewModel.GarageSaleSpecialItems)
                    {
                        viewModel.SelectedCategories.Add(item.ItemSubcategoryId);
                    }
                }

                // Set the selected categories for the regular items.
                if (viewModel.GarageSaleItems.Count > 0)
                {
                    foreach (var item in viewModel.GarageSaleItems)
                    {
                        viewModel.SelectedCategories.Add(item.ItemSubcategoryId);
                    }
                }
            }
            else
            {
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
                ViewBag.SearchResults = viewModel.SearchResults;

                if (viewModel.MappingData != null)
                {
                    if (viewModel.MappingData.Addresses.Count > 0)
                    {
                        ViewBag.Addresses = viewModel.MappingData.Addresses.ToArray();
                        ViewBag.SearchAddress = viewModel.MappingData.StartingAddress;
                        ViewBag.Radius = viewModel.MappingData.Radius;
                        ViewBag.ShowMap = "true";
                    }
                }
            }

            if (Session["UserSession"] != null)
            {
                session = Session["UserSession"] as UserSession;
                viewModel.User = session.User;

                // Set the sign-in flag.
                ViewBag.SignedIn = "true";

                // Get the user's itinerary or itineraries by user id.
                var itineraryRepository = new ItineraryRepository();
                var itineraryList = new List<GarageSaleItinerary>();
                itineraryList = itineraryRepository.GetItinerariesByUserId(viewModel.User.UserId);
                viewModel.GarageSaleItineraries = itineraryList;

                // Default to the user's address in the search box, unless the user entered an address.
                if (viewModel.MappingData == null)
                {
                    if (viewModel.User.Address != null)
                    {
                        var userAddress = viewModel.User.Address.Address1 + " " + viewModel.User.Address.Address2 + " " + viewModel.User.Address.City +
                            " " + viewModel.User.Address.State + " " + viewModel.User.Address.Zip;
                        TempData["UserAddress"] = userAddress.Trim();
                    }
                }
                else
                {
                    TempData["UserAddress"] = viewModel.MappingData.StartingAddress;
                }

                return View("Index2", viewModel); 
            }
            else
            {
                // Set the sign-in flag.
                ViewBag.SignedIn = "false";

                if (Session["SearchData"] != null)
                {
                    ViewBag.NoAddress = "false";

                    var searchData = Session["SearchData"] as string[];
                    ViewBag.SearchRadius = searchData[4].ToString();
                    ViewBag.Community = searchData[5].ToString();
                }
                else
                {
                    ViewBag.NoAddress = "true";
                    ViewBag.SearchRadius = "false";
                    ViewBag.Community = "false";
                }

                return View(viewModel);
            }
        }

        public ActionResult Index2()
        {
            UserSession session = null;
            var repository = new GarageSaleRepository();
            var viewModel = new GarageSaleViewModel();

            if (Session["ViewModel"] != null)
            {
                viewModel = Session["ViewModel"] as GarageSaleViewModel;
            }

            if (Session["SearchData"] != null)
            {
                var searchData = Session["SearchData"] as string[];
                ViewBag.Address = searchData[0].ToString();
                ViewBag.From = searchData[1].ToString();
                ViewBag.To = searchData[2].ToString();
                ViewBag.SearchCriteria = searchData[3].ToString();
                ViewBag.SearchCategory = searchData[4].ToString();
                ViewBag.SearchRadius = searchData[5].ToString();
                ViewBag.Community = searchData[6].ToString();
            }

            if (Session["ShowModal"] != null)
            {
                ViewBag.ShowModal = "true";
                ViewBag.WaypointAddress = Session["WaypointAddress"].ToString();

                // Clean up.
                Session["ShowModal"] = null;
            }

            viewModel.SelectedCategories = new List<int>();
            viewModel.ItemCategories = repository.GetCategoriesAndSubcategories();

            if (viewModel.SearchResults == null)
            {
                // Get some random items for the home page.
                var randomSpecialItems = repository.GetRandomSpecialItems();
                viewModel.GarageSaleSpecialItems = randomSpecialItems;

                // Get random garage sale items for the home page.
                var randomGarageSaleItems = repository.GetRandomGarageSaleItems();
                viewModel.GarageSaleItems = randomGarageSaleItems;

                // Set the selected categories for the special items.
                if (viewModel.GarageSaleSpecialItems.Count > 0)
                {
                    foreach (var item in viewModel.GarageSaleSpecialItems)
                    {
                        viewModel.SelectedCategories.Add(item.ItemSubcategoryId);
                    }
                }

                // Set the selected categories for the regular items.
                if (viewModel.GarageSaleItems.Count > 0)
                {
                    foreach (var item in viewModel.GarageSaleItems)
                    {
                        viewModel.SelectedCategories.Add(item.ItemSubcategoryId);
                    }
                }
            }
            else
            {
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
                ViewBag.SearchResults = viewModel.SearchResults;

                if (viewModel.MappingData != null)
                {
                    if (viewModel.MappingData.Addresses.Count > 0)
                    {
                        ViewBag.Addresses = viewModel.MappingData.Addresses.ToArray();
                        ViewBag.SearchAddress = viewModel.MappingData.StartingAddress;
                        ViewBag.Radius = viewModel.MappingData.Radius;
                        ViewBag.ShowMap = "true";
                    }
                }
            }

            if (Session["UserSession"] != null)
            {
                session = Session["UserSession"] as UserSession;
                viewModel.User = session.User;

                // Set the sign-in flag.
                ViewBag.SignedIn = "true";

                // Get the user's itinerary or itineraries by user id.
                var itineraryRepository = new ItineraryRepository();
                var itineraryList = new List<GarageSaleItinerary>();
                itineraryList = itineraryRepository.GetItinerariesByUserId(viewModel.User.UserId);
                viewModel.GarageSaleItineraries = itineraryList;

                // Default to the user's address in the search box, unless the user entered an address.
                if (viewModel.MappingData == null)
                {
                    if (viewModel.User.Address != null)
                    {
                        var userAddress = viewModel.User.Address.Address1 + " " + viewModel.User.Address.Address2 + " " + viewModel.User.Address.City +
                            " " + viewModel.User.Address.State + " " + viewModel.User.Address.Zip;
                        TempData["UserAddress"] = userAddress.Trim();
                    }
                }
                else
                {
                    TempData["UserAddress"] = viewModel.MappingData.StartingAddress;
                }

                return View(viewModel);
            }
            else
            {
                // Set the sign-in flag.
                ViewBag.SignedIn = "false";

                if (Session["SearchData"] != null)
                {
                    ViewBag.NoAddress = "false";

                    var searchData = Session["SearchData"] as string[];
                    ViewBag.SearchCategory = searchData[4].ToString();
                    ViewBag.SearchRadius = searchData[5].ToString();
                    ViewBag.Community = searchData[6].ToString();
                }
                else
                {
                    ViewBag.NoAddress = "true";
                    ViewBag.SearchRadius = "false";
                    ViewBag.Community = "false";
                }

                return View(viewModel);
            }
        }

        /// <summary>
        /// This displays a garage sale.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QuickViewGarageSale(int id)
        {
            var viewModel = new GarageSaleViewModel();

            if (id == 0)
            {
                return View("~/Views/GarageSale/GarageSaleNotFound.cshtml");
            }
            else
            {
                var repository = new GarageSaleRepository();

                viewModel = new GarageSaleViewModel
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
                }

                // Save the viewmodel for later use.
                Session["ViewModel"] = viewModel;
            }

            ViewBag.NavGarageSales = "active";

            return View(viewModel);
        }

        public ActionResult LogOut()
        {
            var session = Session["UserSession"] as UserSession;
            Session["UserSession"] = null;

            return RedirectToAction("Index");
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
            viewModel.SearchResults = repository.SearchGarageSales(searchCriteria);
            ViewBag.SearchResults = viewModel.SearchResults;
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

            return RedirectToAction("Index", viewModel);
        }

        /// <summary>
        /// This allows the user to search by text criteria, date/time, and category filters.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdvancedSearch(FormCollection form)
        {
            var searchCriteria = string.Empty;

            if (form["txtSearchCriteria"] != null)
            {
                searchCriteria = form["txtSearchCriteria"].ToString();
            }

            var community = form["ddlCommunity"].ToString();
            var radius = form["ddlRadius"].ToString();
            var address = form["txtAddress"].ToString();
            var startDate = form["from"].ToString();
            var endDate = form["to"].ToString();
            var searchCategory = form["ddlCategories"].ToString();

            string[] searchData = { address, startDate, endDate, searchCriteria, searchCategory, radius, community };
            Session["SearchData"] = searchData;

            var categoryIdList = new List<int>();
            int categoryId;

            if (Int32.TryParse(searchCategory, out categoryId))
            {
                categoryIdList.Add(categoryId);
            }
            else
            {
                switch (searchCategory)
                {
                    case "all_baby":
                        {
                            categoryIdList = new List<int>(new int[] {4, 6, 2, 7, 3, 8, 9, 5, 1});
                        }
                    break;
                    case "all_clothing_accessories":
                    {
                        categoryIdList = new List<int>(new int[] { 26, 28, 19, 12, 17, 10, 27, 13, 16, 22, 21, 24, 30, 25, 15, 29, 18, 11, 14, 23, 20 });
                    }
                    break;
                    case "all_baby":
                    {
                        categoryIdList = new List<int>(new int[] { 4, 6, 2, 7, 3, 8, 9, 5, 1 });
                    }
                    break;
                    case "all_baby":
                    {
                        categoryIdList = new List<int>(new int[] { 4, 6, 2, 7, 3, 8, 9, 5, 1 });
                    }
                    break;
                    case "all_baby":
                    {
                        categoryIdList = new List<int>(new int[] { 4, 6, 2, 7, 3, 8, 9, 5, 1 });
                    }
                    break;
                    case "all_baby":
                    {
                        categoryIdList = new List<int>(new int[] { 4, 6, 2, 7, 3, 8, 9, 5, 1 });
                    }
                    break;
                    case "all_baby":
                    {
                        categoryIdList = new List<int>(new int[] { 4, 6, 2, 7, 3, 8, 9, 5, 1 });
                    }
                    break;
                    case "all_baby":
                    {
                        categoryIdList = new List<int>(new int[] { 4, 6, 2, 7, 3, 8, 9, 5, 1 });
                    }
                    break;
                    case "all_baby":
                    {
                        categoryIdList = new List<int>(new int[] { 4, 6, 2, 7, 3, 8, 9, 5, 1 });
                    }
                    break;
                    case "all_baby":
                    {
                        categoryIdList = new List<int>(new int[] { 4, 6, 2, 7, 3, 8, 9, 5, 1 });
                    }
                    break;
                    case "all_baby":
                    {
                        categoryIdList = new List<int>(new int[] { 4, 6, 2, 7, 3, 8, 9, 5, 1 });
                    }
                    break;
                    case "all_baby":
                    {
                        categoryIdList = new List<int>(new int[] { 4, 6, 2, 7, 3, 8, 9, 5, 1 });
                    }
                    break;
                }
            }

            //categoryIdList.Add(Convert.ToInt16(searchCategory));

            // This is for when you allow searching of multiple categories
            //foreach (var key in form.AllKeys)
            //{
            //    int categoryId;

            //    if (Int32.TryParse(key, out categoryId))
            //    {
            //        categoryIdList.Add(categoryId);
            //    }
            //}

            var viewModel = new GarageSaleViewModel();            

            if (Session["ViewModel"] != null)
            {
                viewModel = Session["ViewModel"] as GarageSaleViewModel;
            }

            viewModel.MappingData = new MappingData();
            viewModel.MappingData.StartingAddress = address;
            viewModel.MappingData.Radius = radius;
            viewModel.MappingData.Addresses = new List<string>();

            var repository = new GarageSaleRepository();
            if (community == "all")
            {
                viewModel.SearchResults = repository.SearchGarageSales(searchCriteria, categoryIdList, startDate, endDate);
            }
            else
            {
                viewModel.SearchResults = repository.SearchGarageSalesByCommunity(community, searchCriteria, categoryIdList, startDate, endDate);
            }

            // Instantiate the selected categories.
            viewModel.SelectedCategories = new List<int>();

            // Take all the items in the search results and piece together addresses.
            foreach (var item in viewModel.SearchResults.GarageSaleItems)
            {
                viewModel.MappingData.Addresses.Add(item.Address1 + ' ' + item.Address2 + ' ' + item.City + ' ' + item.State + ' ' + item.ZipCode);                
                viewModel.SelectedCategories.Add(item.ItemSubcategoryId);
            }

            // Get the special items addresses
            if (viewModel.SearchResults.SpecialItems.Count > 0)
            {
                foreach (var item in viewModel.SearchResults.SpecialItems)
                {
                    var saleAddress = repository.GetGarageSaleAddressBySaleId(item.SaleId);
                    viewModel.MappingData.Addresses.Add(saleAddress.Address1 + ' ' + saleAddress.Address2 + ' ' + saleAddress.City + ' ' + saleAddress.State + ' ' + saleAddress.ZipCode);
                    viewModel.SelectedCategories.Add(item.ItemSubcategoryId);
                }
            }

            var selectedCategories = viewModel.SelectedCategories.ToArray();
            ViewBag.SelectedCategories = string.Join(",", selectedCategories);

            Session["ViewModel"] = viewModel;
            TempData["SearchButtonClicked"] = "true";

            return RedirectToAction("Index2", viewModel);
        }

        /// <summary>
        /// This gets the item categories by subcategory.
        /// </summary>
        /// <param name="itemSubcategoryId"></param>
        /// <returns></returns>
        public ActionResult GetCategoriesByCategoryId(int categoryId)
        {
            var viewModel = new GarageSaleViewModel();

            var repository = new GarageSaleRepository();
            viewModel.ItemCategories = repository.GetCategoriesAndSubcategories();
            viewModel.FilteredItemCategories = repository.GetCategoriesAndSubcategoriesById(categoryId);

            Session["ViewModel"] = viewModel;

            return View("~/Views/Shared/_CategoriesMenu.cshtml", viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SetStopoverAddress(string address)
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

                // Get the user's address.
                if (viewModel.User.Address != null)
                {
                    ViewBag.CenterAddress = viewModel.User.Address.Address1 + " " + viewModel.User.Address.Address2 + " " + viewModel.User.Address.City + " " +
                        viewModel.User.Address.State + " " + viewModel.User.Address.Zip;
                }
            }

            Session["ShowModal"] = "true";
            Session["WaypointAddress"] = address;
            
            return RedirectToAction("Index", viewModel);
        }
    }
}
