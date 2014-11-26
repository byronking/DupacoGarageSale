using DupacoGarageSale.Data.Domain;
using DupacoGarageSale.Data.Repository;
using DupacoGarageSale.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DupacoGarageSale.Web.Controllers
{
    public class AccountsController : Controller
    {
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

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
                    UserId = user.UserId,
                    UserName = user.UserName
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

        [HttpGet]
        public ActionResult UserProfile(int? id)
        {
            // If the user id is null, then attempt to log the user in. Else, retrieve the user by id.
            var repository = new AccountsRepository();
            var user = new GarageSaleUser();

            if (id == null)
            {
                
            }
            else
            {
                user = repository.GetUserProfileInfoById((int)id);
            }

            return View(user);
        }

        
    }
}
