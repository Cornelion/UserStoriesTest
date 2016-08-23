using Helpers.DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace UserStoriesTest.Controllers
{
    public class UsersController : Controller
    { public Registry registry = new Registry();
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (registry.CheckIfUserExist(user))
            {
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                   1, user.UserName, DateTime.Now, DateTime.Now.AddMinutes(20), false, Guid.NewGuid().ToString());
                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie logCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(logCookie);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        
    }
}