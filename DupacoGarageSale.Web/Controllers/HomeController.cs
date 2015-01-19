using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Repository;
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
            viewModel.ItemCategories = repository.GetCategoriesAndSubcategories();

            // Get some random items for the home page.
            var randomSpecialItems = ItemsHelper.GetRandomSpecialItems();
            var randomGarageSaleItems = ItemsHelper.GetRandomGarageSaleItems();

            viewModel.GarageSaleSpecialItems = repository.GetGarageSaleSpecialItems(randomSpecialItems);

            if (Session["UserSession"] != null)
            {
                session = Session["UserSession"] as UserSession;
                viewModel.User = session.User;

                return View(viewModel);
            }
            else
            {
                return View(viewModel);
            }
        }

        public ActionResult LogOut()
        {
            var session = Session["UserSession"] as UserSession;
            Session["UserSession"] = null;

            return RedirectToAction("Index");
        }
    }
}
