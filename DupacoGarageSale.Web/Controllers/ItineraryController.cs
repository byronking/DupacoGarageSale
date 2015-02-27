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
            //var viewModel = new GarageSaleViewModel();

            //if (Session["ViewModel"] != null)
            //{
            //    viewModel = Session["ViewModel"] as GarageSaleViewModel;
            //}

            // Check to see if the user has an itinerary. If not, create a new one. If so, update the existing one.
            var repository = new ItineraryRepository();

            var itinerary = repository.CheckForItinerary(viewModel.UserItinerary.ItineraryOwner);

            if (itinerary.ItineraryId != 0)
            {
                // Add a new leg to the existing itinerary.
                //var saveResult = repository.SaveItineraryLeg(itinerary.ItineraryId, saleId);
            }
            else
            {
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
                viewModel.GarageSaleItinerary = itineraryList;

                //if (Session["ItineraryLegDeleted"] != null)
                //{
                //    var deleteSuccessful = Convert.ToBoolean(Session["ItineraryLegDeleted"]);

                //    if (deleteSuccessful)
                //    {
                //        ViewBag.Invisible = "false";
                //    }
                //}

                //// Get the user's fave garage sales.
                //var saleRepository = new GarageSaleRepository();
                //viewModel.FavoriteGarageSales = saleRepository.GetFavoriteGarageSales(viewModel.User.UserId);

                //// Cleanup.
                //Session["ItineraryLegDeleted"] = null;

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
                viewModel.GarageSaleItinerary = itineraryList;

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
        public ActionResult ViewItinerary(int? id)
        {
            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;

                // Get the user's itinerary by user id.
                var repository = new ItineraryRepository();
                var itineraryList = new List<GarageSaleItinerary>();

                itineraryList = repository.GetItinerariesByItineraryId((int)id);

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
        /// 
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
        /// This moves an itinerary leg up in the order.
        /// </summary>
        /// <param name="legId"></param>
        /// <param name="legOrder"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This moves an itinerary leg down in the order.
        /// </summary>
        /// <param name="legId"></param>
        /// <param name="legOrder"></param>
        /// <returns></returns>
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
