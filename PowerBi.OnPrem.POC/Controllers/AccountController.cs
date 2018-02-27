using Newtonsoft.Json;
using PowerBi.OnPrem.POC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PowerBi.OnPrem.POC.Controllers
{
    public class AccountController : Controller
    {
        SampleDbContext context;
        public AccountController()
        {
            context = new SampleDbContext();
        }
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Dummy user retrieval POC use only
            User user = context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }

            FormsAuthentication.SetAuthCookie(model.Username, false);

            var userData = new UserData
            {
                UserId = user.UserId,
                Roles = user.IsAdmin ? new[] { "Admin", "User" } : new[] { "User" }
            };

            var userDataJson = JsonConvert.SerializeObject(userData);
            var authTicket = new FormsAuthenticationTicket(1, model.Username, DateTime.Now, DateTime.Now.AddMinutes(30), false, userDataJson);
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);

            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}