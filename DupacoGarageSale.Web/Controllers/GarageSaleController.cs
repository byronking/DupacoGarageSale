using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DupacoGarageSale.Web.Controllers
{
    public class GarageSaleController : Controller
    {
        [HttpGet]
        public ActionResult Add()
        {
            var userSession = new UserSession();

            if (Session["UserSession"] != null)
            {
                userSession = Session["UserSession"] as UserSession;

                var viewModel = new GarageSaleViewModel
                {
                    User = userSession.User
                };

                ViewBag.NavAddSale = "active";
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("SignIn", "Accounts");
            }            
        }
    }
}