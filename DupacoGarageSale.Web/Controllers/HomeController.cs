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

                // Default to the user's address in the search box, unless the user entered an address.
                if (viewModel.MappingData == null)
                {
                    var userAddress = viewModel.User.Address.Address1 + " " + viewModel.User.Address.Address2 + " " + viewModel.User.Address.City +
                        " " + viewModel.User.Address.State + " " + viewModel.User.Address.Zip;
                    TempData["UserAddress"] = userAddress.Trim();
                }
                else
                {
                    TempData["UserAddress"] = viewModel.MappingData.StartingAddress;
                }

                return View(viewModel);
            }
            else
            {
                ViewBag.NoAddress = "true";
                return View(viewModel);
            }
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

        // This allows the user to filter search results.
        [HttpPost]
        public ActionResult FilterResults(FormCollection form)
        {
            var searchCriteria = string.Empty;

            if (form["txtSearchWithFilters"] != null)
            {
                searchCriteria = form["txtSearchWithFilters"].ToString();
            }

            var radius = form["ddlRadius"].ToString();
            var address = form["txtAddress"].ToString();
            var startDate = form["from"].ToString();
            var endDate = form["to"].ToString();

            #region Old code using the checkboxes
            //var saleDates = new Dictionary<string, string>();
            //var saleDateOne = "off";
            //if (form["saleDateOne"] != null)
            //{
            //    saleDateOne = form["saleDateOne"].ToString();
            //    saleDates.Add("saleDateOne", System.Configuration.ConfigurationManager.AppSettings["SaleDateOne"].ToString());
            //}

            //var saleDateTwo = "off";
            //if (form["saleDateTwo"] != null)
            //{
            //    saleDateTwo = form["saleDateTwo"].ToString();
            //    saleDates.Add("saleDateTwo", System.Configuration.ConfigurationManager.AppSettings["SaleDateTwo"].ToString());
            //}

            //var saleDateThree = "off";
            //if (form["saleDateThree"] != null)
            //{
            //    saleDateThree = form["saleDateThree"].ToString();
            //    saleDates.Add("saleDateThree", System.Configuration.ConfigurationManager.AppSettings["SaleDateThree"].ToString());
            //}

            //var saleDateFour = "off";
            //if (form["saleDateFour"] != null)
            //{
            //    saleDateFour = form["saleDateFour"].ToString();
            //    saleDates.Add("saleDateFour", System.Configuration.ConfigurationManager.AppSettings["SaleDateFour"].ToString());
            //}
            #endregion

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

            viewModel.MappingData = new MappingData();
            viewModel.MappingData.StartingAddress = address;
            viewModel.MappingData.Radius = radius;
            viewModel.MappingData.Addresses = new List<string>();

            var repository = new GarageSaleRepository();
            viewModel.SearchResults = repository.SearchGarageSales(searchCriteria, categoryIdList, startDate, endDate);            

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

            return RedirectToAction("Index", viewModel);
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
    }
}
