using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Repository;
using DupacoGarageSale.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DupacoGarageSale.Web.Controllers
{
    public class AccountsController : Controller
    {
        /// <summary>
        /// This loads the sign up page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        /// <summary>
        /// This saves a newly registered user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegisterUser(GarageSaleUser model)
        {
            var errors = ModelState.Where(v => v.Value.Errors.Any());

            model.CreateDate = DateTime.Now;
            model.ModifyUser = model.UserName;
            model.Active = true;

            if (ModelState.IsValid)
            {
                var saveResult = new UserSaveResult();

                // Save the new user and then send them to the profile page.
                var repository = new AccountsRepository();
                saveResult = repository.SaveGarageSaleUser(model);

                model.UserId = saveResult.SaveResultId;
            }

            return RedirectToAction("UserProfile", new RouteValueDictionary(new
            {
                controller = "Accounts",
                action = "UserProfile",
                id = model.UserId
            }));
        }

        /// <summary>
        /// This is the default user login page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SignIn()
        {
            return View("~/Views/Accounts/Login.cshtml");
        }

        /// <summary>
        /// This signs in a user.
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SignIn(FormCollection formCollection)
        {
            var model = new AuthenticationInfo
            {
                UserName = formCollection["UserName"].ToString(),
                Password = formCollection["Password"].ToString()
            };

            // Hash the password, then attempt a match.
            var hashedPassword = AccountHelper.GetSHA1Hash(model.UserName, model.Password);
            var repository = new AccountsRepository();
            var user = repository.GetActiveGarageSaleUserByUserName(model.UserName, hashedPassword);

            if (model.UserName == user.UserName)
            {
                //var retrievedUserHashedPassword = AccountHelper.GetSHA1Hash(user.UserName, user.BytePassword);
                var matchedUser = AccountHelper.MatchSHA1(hashedPassword, user.BytePassword);

                // Create a user session.
                var session = new UserSession
                {
                    SessionKey = Guid.NewGuid(),
                    SessionStartDate = DateTime.Now,
                    User = user
                };

                Session["UserSession"] = session;

                return RedirectToAction("UserProfile", new RouteValueDictionary(new
                {
                    controller = "Accounts",
                    action = "UserProfile",
                    id = user.UserId
                }));
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        /// <summary>
        /// This loads a user profile by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserProfile(int? id)
        {
            var user = new GarageSaleUser();

            if (Session["UserSession"] != null)
            {
                // Load the states dropdown.
                var addressRepository = new AddressRepository();
                var statesList = addressRepository.GetStates();
                ViewData["StatesList"] = new SelectList(statesList, "stateid", "statename");

                // If the user id is null, then attempt to log the user in. Else, retrieve the user by id.
                var repository = new AccountsRepository();

                if (id == null)
                {
                    // Load an empty form for new users.
                }
                else
                {
                    // Load a saved user by id.
                    user = repository.GetUserProfileInfoById((int)id);

                    // Set the default profile pic
                    if (user.ProfilePicLink == string.Empty)
                    {
                        user.ProfilePicLink = "keep-calm-and-come-to-the-dupaco-garage-sale.png";
                    }
                }

                ViewBag.NavProfile = "active";

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

                return View(user);
            }
            else
            {
                return RedirectToAction("SignIn", "Accounts");
            }
        }

        /// <summary>
        /// This saves changes to the registered user profile.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveUserProfile(HttpPostedFileBase fileUpload, GarageSaleUser user)
        {
            user.ModifyDate = DateTime.Now;
            user.ModifyUser = user.UserName;

            if (fileUpload != null)
            {
                // Save the image file.
                user.ProfilePicLink = fileUpload.FileName;

                var fileName = Path.GetFileName(user.ProfilePicLink);
                var dir = ConfigurationManager.AppSettings["ProfileImagesDirectory"].ToString();

                var storageDir = dir + Path.DirectorySeparatorChar + fileName;

                if (!System.IO.File.Exists(fileName))
                {
                    fileUpload.SaveAs(dir + Path.DirectorySeparatorChar + fileName);
                }
            }
            else
            {
                // Use the default pic.
                user.ProfilePicLink = "keep-calm-and-come-to-the-dupaco-garage-sale.png";
            }

            var errors = ModelState.Where(v => v.Value.Errors.Any());

            var saveResult = new UserSaveResult();
            var repository = new AccountsRepository();
            saveResult = repository.SaveGarageSaleUserProfile(user);

            if (saveResult.IsSaveSuccessful)
            {
                Session["SaveSuccessful"] = true;
            }

            return RedirectToAction("UserProfile", new RouteValueDictionary(new
            {
                controller = "Accounts",
                action = "UserProfile",
                id = user.UserId
            }));
        }
    }
}