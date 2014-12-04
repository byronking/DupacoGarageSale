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

                if (id == null)
                {
                    // Load an empty form for new users.
                }
                else
                {
                    // Load a saved garage sale by id.
                    viewModel.Sale = repository.GetGarageSaleById((int)id);
                }

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
        public ActionResult Save(GarageSaleViewModel model)
        {
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

            if (ModelState.IsValid)
            {
                var saveResult = new UserSaveResult();

                var repository = new GarageSaleRepository();
                saveResult = repository.SaveGarageSale(model.Sale);
                model.Sale.GarageSaleId = saveResult.SaveResultId;
            }

            ViewBag.NavAddSale = "active";

            return RedirectToAction("Add", new RouteValueDictionary(new
            {
                controller = "GarageSale",
                action = "Add",
                id = model.Sale.GarageSaleId
            }));
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
    }
}