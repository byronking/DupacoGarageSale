using DupacoGarageSale.Data.Domain;
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
            return View();
        }

        public ActionResult LogOut()
        {
            var session = Session["UserSession"] as UserSession;
            Session["UserSession"] = null;

            return RedirectToAction("Index");
        }
    }
}
