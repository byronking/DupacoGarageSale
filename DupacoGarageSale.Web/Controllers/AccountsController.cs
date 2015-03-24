using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Repository;
using DupacoGarageSale.Data.Services;
using DupacoGarageSale.Domain.Helpers;
using DupacoGarageSale.Web.Helpers;
using DupacoGarageSale.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mail;
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
            UserSession session = null;

            var viewModel = new GarageSaleViewModel();

            if (Session["UserSession"] != null)
            {
                session = Session["UserSession"] as UserSession;
                viewModel.User = session.User;
                return View(viewModel);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// This saves a newly registered user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegisterUser(GarageSaleViewModel model)
        {
            var errors = ModelState.Where(v => v.Value.Errors.Any());

            model.User.CreateDate = DateTime.Now;
            model.User.ModifyUser = model.User.UserName;
            model.User.Active = true;

            // Make sure new accounts are unique.
            var repository = new AccountsRepository();

            if (repository.CheckForExistingAccount(model.User.UserName, model.User.Email))
            {
                ViewBag.IsExistingAccount = "true";
                ViewBag.Username = model.User.UserName;
                ViewBag.Email = model.User.Email;
                return View("~/Views/Accounts/SignUp.cshtml", model);
            }
            else
            {
                var saveResult = new UserSaveResult();

                // Save the new user and then send them to the profile page.
                saveResult = repository.SaveGarageSaleUser(model.User);
                model.User.UserId = saveResult.SaveResultId;

                if (model.User.UserId == 0)
                {
                    return RedirectToAction("ProfileCreationError", new RouteValueDictionary(new
                    {
                        controller = "Accounts",
                        action = "ProfileCreationError"
                    }));
                }
                else
                {
                    // Create a user session.
                    var session = new UserSession
                    {
                        SessionKey = Guid.NewGuid(),
                        SessionStartDate = DateTime.Now,
                        User = model.User
                    };

                    Session["UserSession"] = session;

                    return RedirectToAction("UserProfile", new RouteValueDictionary(new
                    {
                        controller = "Accounts",
                        action = "UserProfile",
                        id = model.User.UserId
                    }));
                }
            }
        }

        /// <summary>
        /// This responds to an error with user profile creation.
        /// </summary>
        /// <returns></returns>
        public ActionResult ProfileCreationError()
        {
            return View();
        }

        /// <summary>
        /// This is the default user login page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login(string redirectId)
        {
            if (redirectId != null)
            {
                // do something
                Session["RedirectId"] = redirectId;
                return View("~/Views/Accounts/Login.cshtml");
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        /// <summary>
        /// This signs in a user.
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(FormCollection formCollection)
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

                // Get the user's address if they log in.
                session.User.Address = repository.GetUserAddressByUserId(session.User.UserId);
                Session["UserSession"] = session;

                if (Session["RedirectId"] != null)
                {
                    var redirectId = Convert.ToInt32(Session["RedirectId"]);

                    return RedirectToAction("ViewGarageSale", new RouteValueDictionary(new
                    {
                        controller = "GarageSale",
                        action = "ViewGarageSale",
                        id = redirectId
                    }));
                }
                else
                {
                    return RedirectToAction("UserHome", new RouteValueDictionary(new
                    {
                        controller = "Accounts",
                        action = "UserHome",
                        id = session.User.UserId
                    }));
                }
            }
            else
            {
                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        [HttpPost]
        public ActionResult SignInFacebookUser(string id, string first_name, string last_name, string email)
        {
            // Verify an existing user account or send to the login screen.
            var repository = new AccountsRepository();

            // Create a user session.
            var user = repository.GetGarageSaleUserByUserName(id);
            user.Address = repository.GetUserAddressByUserId(user.UserId);

            var saveResult = new UserSaveResult();

            if (user.UserId == 0)
            {
                user = new GarageSaleUser
                {
                    Active = true,
                    Email = email,
                    FirstName = first_name,
                    LastName = last_name,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    ModifyUser = id,
                    UserName = id,
                    Password = "facebook" + id,
                    Phone = "563-555-1212"
                };

                // Save the new user and then send them to the profile page.
                repository = new AccountsRepository();
                saveResult = repository.SaveGarageSaleUser(user);
                user.UserId = saveResult.SaveResultId;
            }

            // Create a user session.
            var session = new UserSession
            {
                SessionKey = Guid.NewGuid(),
                SessionStartDate = DateTime.Now,
                User = user
            };            

            Session["UserSession"] = session;
            ViewBag.UserId = user.UserId;

            return Json(new { ok = true, newurl = Url.Action("UserHome", new { id = user.UserId })});
        }

        /// <summary>
        /// This is the user's home page.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserHome(int? id)
        {
            var viewModel = new GarageSaleViewModel();
            var user = new GarageSaleUser();

            if (Session["UserSession"] != null)
            {
                // Load a saved user by id.
                var repository = new AccountsRepository();
                user = repository.GetUserProfileInfoById((int)id);
                viewModel.User = user;

                // Set the default profile pic
                if (user.ProfilePicLink == string.Empty)
                {
                    user.ProfilePicLink = "keep-calm-and-come-to-the-dupaco-garage-sale.png";
                }

                var garageSaleRepository = new GarageSaleRepository();
                viewModel.GarageSales = garageSaleRepository.GetGarageSaleByUserName(viewModel.User.UserName);
                viewModel.FavoriteGarageSales = garageSaleRepository.GetFavoriteGarageSales(viewModel.User.UserId);

                // Load any headlines, if any.
                var adminRepository = new AdminRepository();
                var adminMessages = adminRepository.GetHeadlineNews();
                if (adminMessages.Count > 0)
                {
                    viewModel.HeadlineNews = adminMessages[0].MessageText;
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

                Session["ViewModel"] = viewModel;

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
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
                return RedirectToAction("Login", "Accounts");
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
                user.ProfilePicLink = DateTime.Now.ToString("yyyyMMddHHmmss") + fileUpload.FileName;

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
                if (user.ProfilePicLink == null)
                {
                    // Use the default pic.
                    user.ProfilePicLink = "keep-calm-and-come-to-the-dupaco-garage-sale.png";
                }
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

        /// <summary>
        /// This saves the profile pic to a temporary place for IE < 10 browsers.
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveProfilePicForPreview(HttpPostedFileBase fileUpload)
        {

            return View("UserProfile");
        }

        /// <summary>
        /// This resets the user's password.
        /// </summary>
        /// <param name="accountInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(PasswordResetInfo accountInfo)
        {
            // Verify that the account info is legit.
            if (AccountHelper.ValidateUserAccount(accountInfo))
            {
                // Generate a reset token and store it in the database.
                var token = AccountHelper.GeneratePasswordResetToken(20);

                var repository = new AccountsRepository();
                var passwordResetRequest = new PasswordResetRequest
                {
                    UserName = accountInfo.UserName,
                    Email = accountInfo.Email,
                    ResetToken = token,
                    RequestDateTime = DateTime.Now
                };

                var saveResult = repository.SavePasswordResetRequest(passwordResetRequest);

                if (saveResult.IsSaveSuccessful == true)
                {
                    try
                    {
                        // Send password reset email.
                        var mailMessage = new System.Net.Mail.MailMessage("Password Reset <password-reset@dupacogaragesales.com>", passwordResetRequest.Email);
                        mailMessage.IsBodyHtml = true;
                        mailMessage.Subject = "Password reset request";
                        mailMessage.Body = @"Here is your password reset request.  Use this link to reset your password: " + 
                            ConfigurationManager.AppSettings["PasswordResetUrl"].ToString() + token;                        
                        mailMessage.Priority = System.Net.Mail.MailPriority.Normal;

                        var smtp = new SmtpClient();
                        smtp.Host = "localhost"; 
                        smtp.Send(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(ex.ToString());
                    }
                }
            }
            else
            {
                // Invalid account entered.
                Logger.Log.Warn("Invalid email for password reset request: " + accountInfo.Email);
            }

            return View();
        }

        public ActionResult ChangePassword(FormCollection form)
        {
            var password = form["Password"].ToString();

            if (Session["UserSession"] != null)
            {
                var session = Session["UserSession"] as UserSession;
                session.User.Password = password;

                // Update the user's password.
                var repository = new AccountsRepository();
                var hashedPassword = AccountHelper.GetSHA1Hash(session.User.UserName, password);

                try
                {
                    var saveSuccessful = repository.UpdateUserPassword(session.User.UserName, hashedPassword);

                    if (saveSuccessful)
                    {
                        Session["SaveSuccessful"] = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.ToString());
                }

                return RedirectToAction("UserProfile", new RouteValueDictionary(new
                {
                    controller = "Accounts",
                    action = "UserProfile",
                    id = session.User.UserId
                }));
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }

        [HttpGet]
        public ActionResult ProcessPasswordReset(string t)
        {
            var token = string.Empty;

            if (t.StartsWith("3D="))
            {
                token = t.Remove(0, 3);
            }
            else
            {
                token = t;
            }

            // Get the user associated with the request.
            var repository = new AccountsRepository();
            var request = repository.GetUserByResetToken(token);
            var requestExpiration = request.RequestDateTime.AddHours(24);

            if (DateTime.Now > requestExpiration)
            {
                // The request is too old!
                ViewBag.ShowError = "visible";
                Logger.Log.Warn("Password reset request expired or invalid for " + request.UserName + " - " + request.Email);
            }

            return View(request);
        }

        [HttpPost]
        public ActionResult ProcessPasswordReset(PasswordResetRequest request)
        {
            // Hash, then update the password.
            var hashedPassword = AccountHelper.GetSHA1Hash(request.UserName, request.User.Password);
            var repository = new AccountsRepository();

            try
            {
                var saveSuccessful = repository.UpdateUserPassword(request.UserName, hashedPassword);

                if (saveSuccessful)
                {
                    TempData["PasswordResetSuccessful"] = "visible";
                }
                else
                {
                    TempData["PasswordResetSuccessful"] = "hidden";
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }

            return RedirectToAction("Login");
        }
    }
}