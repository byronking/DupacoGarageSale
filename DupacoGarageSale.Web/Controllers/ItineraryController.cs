using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Repository;
using DupacoGarageSale.Web.Models;
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
        /// <summary>
        /// This creates a new or adds a sale to an existing itinerary.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateOrAddToItinerary(GarageSaleViewModel viewModel)
        {
            // Check to see if the user has an itinerary. If not, create a new one. If so, update the existing one.
            var repository = new ItineraryRepository();

            var itinerary = repository.CheckForItinerary(viewModel.UserItinerary.ItineraryOwner);

            // Create a new itinerary.
            itinerary = new Itinerary
            {
                ItineraryName = viewModel.UserItinerary.ItineraryName,
                SaleId = viewModel.Sale.GarageSaleId,
                ItineraryCreateDate = DateTime.Now,
                ItineraryModifyDate = DateTime.Now,
                ItineraryOwner = viewModel.User.UserId
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

            // If there is a waypoint address, add it here.
            if (Session["WaypointAddress"] != null)
            {
                var address = Session["WaypointAddress"].ToString();
                var waypointSaveResult = repository.SaveItineraryWaypoints(address, itinerary.ItineraryId);

                if (saveResult.IsSaveSuccessful)
                {
                    Session["WaypointSaved"] = true;
                }
            }

            Session["ViewModel"] = viewModel;

            return RedirectToAction("ViewItinerary", new RouteValueDictionary(new
            {
                id = itinerary.ItineraryId
            }));
        }

        /// <summary>
        /// This gets the user's itineraries by user id.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserItineraries(int id)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                // Get the user's itinerary by user id.
                var repository = new ItineraryRepository();
                var itineraryList = new List<GarageSaleItinerary>();

                itineraryList = repository.GetItinerariesByUserId(id);

                // Add it to the viewModel.
                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                }

                viewModel.User = session.User;
                viewModel.GarageSaleItineraries = itineraryList;

                if (Session["ItineraryDeleted"] != null)
                {
                    var deleteSuccessful = Convert.ToBoolean(Session["ItineraryDeleted"]);

                    if (deleteSuccessful)
                    {
                        ViewBag.ItineraryDeleted = "true";
                    }
                }

                // Cleanup
                Session["ItineraryDeleted"] = null;

                ViewBag.NavCreateItinerary = "active";

                Session["ViewModel"] = viewModel;
                return View("UserItineraries", viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        /// <summary>
        /// This loads the user's itinerary.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetItineraryByItineraryId(int? id)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                // Get the user's itinerary by user id.
                var repository = new ItineraryRepository();
                var itineraryList = new List<GarageSaleItinerary>();

                if (id != null)
                {
                    itineraryList = repository.GetItinerariesByUserId(Convert.ToInt32(id));
                }
                else
                {
                    itineraryList = repository.GetItinerariesByUserId(session.User.UserId);
                }

                // Add it to the viewModel.
                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                }

                viewModel.User = session.User;
                viewModel.GarageSaleItineraries = itineraryList;

                if (Session["ItineraryLegDeleted"] != null)
                {
                    var deleteSuccessful = Convert.ToBoolean(Session["ItineraryLegDeleted"]);

                    if (deleteSuccessful)
                    {
                        ViewBag.Invisible = "false";
                    }
                }

                // Get the user's fave garage sales.
                var saleRepository = new GarageSaleRepository();
                viewModel.FavoriteGarageSales = saleRepository.GetFavoriteGarageSales(viewModel.User.UserId);

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
        /// This loads the user's itinerary.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewItinerary(int id)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                // Get the user's itinerary by user id.
                var repository = new ItineraryRepository();
                var itineraryList = new List<GarageSaleItinerary>();

                itineraryList = repository.GetItinerariesByItineraryId(id);

                // Add it to the viewModel.
                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                }

                viewModel.User = session.User;

                // Add the current itinerary as well.
                viewModel.GarageSaleItinerary = repository.GetItineraryByItineraryId(id);

                if (viewModel.GarageSaleItinerary.ItineraryId == 0)
                {
                    viewModel.GarageSaleItinerary.ItineraryId = id;
                }

                viewModel.GarageSaleItineraries = itineraryList;

                // Get the waypoints, if any and then add them to the itineraries list.
                var waypoints = repository.GetItineraryWaypoints(id);

                foreach (var waypoint in waypoints)
                {
                    var itinerary = new GarageSaleItinerary
                    {
                        SaleId = 7777777,
                        SaleAddress1 = waypoint
                    };

                    viewModel.GarageSaleItineraries.Add(itinerary);
                }

                if (Session["ItineraryLegDeleted"] != null)
                {
                    var deleteSuccessful = Convert.ToBoolean(Session["ItineraryLegDeleted"]);

                    if (deleteSuccessful)
                    {
                        ViewBag.Invisible = "false";
                    }
                }

                // Get the user's fave garage sales.
                var saleRepository = new GarageSaleRepository();
                viewModel.FavoriteGarageSales = saleRepository.GetFavoriteGarageSales(viewModel.User.UserId);

                if (Session["NoResultsReturned"] != null)
                {
                    if(Session["NoResultsReturned"].ToString() == "true")
                    {
                        ViewBag.NoResultsReturned = "true";
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
        /// This deletes the 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteItinerary(int id)
        {
            if (Session["ViewModel"] != null)
            {
                var viewModel = Session["ViewModel"] as GarageSaleViewModel;

                var user = viewModel.User;

                // Delete the itinerary by id.
                var repository = new ItineraryRepository();
                var saveResult = repository.DeleteItinerary(id, user.UserId);

                if (saveResult.IsSaveSuccessful)
                {
                    Session["ItineraryDeleted"] = true;
                }

                return RedirectToAction("GetUserItineraries", new RouteValueDictionary(new
                {
                    controller = "Itinerary",
                    action = "GetUserItineraries",
                    id = user.UserId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        /// <summary>
        /// THIS IS NOT IN USE!!
        /// </summary>
        /// <param name="legId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This deletes an itinerary leg from the itinerary page.
        /// </summary>
        /// <param name="legId"></param>
        /// <returns></returns>
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
                    var saveResult = repository.DeleteItineraryLeg(legId, viewModel.GarageSaleItineraries[0].ItineraryId, viewModel.GarageSaleItineraries[0].SaleId,
                        session.User.UserId);

                    if (saveResult.IsSaveSuccessful)
                    {
                        Session["ItineraryLegDeleted"] = true;
                    }
                }

                return RedirectToAction("ViewItinerary", new RouteValueDictionary(new
                {
                    controller = "Itinerary",
                    action = "ViewItinerary",
                    id = viewModel.GarageSaleItineraries[0].ItineraryId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        /// <summary>
        /// This removes a leg from the user's itinerary.
        /// </summary>
        /// <param name="itineraryId"></param>
        /// <param name="saleId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveFromItinerary(int itineraryId, int saleId)
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
                    var saveResult = repository.DeleteFromItinerary(itineraryId, saleId);

                    if (saveResult.IsSaveSuccessful)
                    {
                        Session["ItineraryLegDeleted"] = true;
                    }
                }

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

        /// <summary>
        /// This is the search page from the itinerary page.
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ItineraryPageSearch(FormCollection formCollection)
        {
            var viewModel = new GarageSaleViewModel();
            var itineraryId = Convert.ToInt32(formCollection["hdnItineraryId"]);

            if (Session["ViewModel"] != null)
            {
                viewModel = Session["ViewModel"] as GarageSaleViewModel;
            }

            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                viewModel.User = session.User;

                var repository = new ItineraryRepository();
                var searchCriteria = formCollection["txtSearchCriteria"].ToString();                
                viewModel.ItinerarySearchResults = repository.ItineraryPageSearch(searchCriteria);

                if (viewModel.ItinerarySearchResults.ItineraryGarageSaleItems.Count == 0 && viewModel.ItinerarySearchResults.ItinerarySpecialItems.Count == 0)
                {
                    Session["NoResultsReturned"] = "true";
                }
            }

            Session["ViewModel"] = viewModel;

            return RedirectToAction("ViewItinerary", new RouteValueDictionary(new
                {
                    controller = "Itinerary",
                    action = "ViewItinerary",
                    id = itineraryId
                }));
        }

        /// <summary>
        /// This allows users to add a waypoint to the itinerary that is not a garage sale.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddStopover()
        {
            var viewModel = new GarageSaleViewModel();

            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                    viewModel.User = session.User;
                }

                Session["ViewModel"] = viewModel;
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        /// <summary>
        /// This saves the address of the waypoint selected by the user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddItineraryWaypoint(int id, string address)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new GarageSaleViewModel();                

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                    viewModel.User = session.User;
                }

                var repository = new ItineraryRepository();
                var saveResult = repository.SaveItineraryWaypoints(address, id);

                if (saveResult.IsSaveSuccessful)
                {
                    Session["WaypointSaved"] = true;
                }

                Session["ViewModel"] = viewModel;

                return RedirectToAction("ViewItinerary", new RouteValueDictionary(new
                {
                    controller = "Itinerary",
                    action = "ViewItinerary",
                    id = viewModel.GarageSaleItinerary.ItineraryId
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        /// <summary>
        /// This deletes a waypoint from the user's itinerary.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public ActionResult DeleteWaypoint(int id, string address)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                var viewModel = new GarageSaleViewModel();

                if (Session["ViewModel"] != null)
                {
                    viewModel = Session["ViewModel"] as GarageSaleViewModel;
                    viewModel.User = session.User;
                }

                var repository = new ItineraryRepository();
                var saveResult = repository.DeleteItineraryWaypoints(address, id);

                if (saveResult.IsSaveSuccessful)
                {
                    Session["WaypointSaved"] = true;
                }

                return RedirectToAction("ViewItinerary", new RouteValueDictionary(new
                {
                    controller = "Itinerary",
                    action = "ViewItinerary",
                    id = id
                }));
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }
    }
}
