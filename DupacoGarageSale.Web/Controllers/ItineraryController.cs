using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DupacoGarageSale.Web.Controllers
{
    public class ItineraryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveItineraryLeg(int legId)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                return RedirectToAction("ViewItinerary", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "ViewItinerary",
                    id = session.User.UserId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        public ActionResult MoveUp(int legId, int legOrder)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                var repository = new ItineraryRepository();
                var saveResult = repository.DecreaseItineraryLegOrder(legId, legOrder);

                return RedirectToAction("ViewItinerary", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "ViewItinerary",
                    id = session.User.UserId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        public ActionResult MoveDown(int legId, int legOrder)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                var repository = new ItineraryRepository();
                var saveResult = repository.IncreaseItineraryLegOrder(legId, legOrder);

                return RedirectToAction("ViewItinerary", new RouteValueDictionary(new
                {
                    controller = "GarageSale",
                    action = "ViewItinerary",
                    id = session.User.UserId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }
    }
}
