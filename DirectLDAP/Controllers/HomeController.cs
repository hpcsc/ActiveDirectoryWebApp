using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DirectLDAP.Models;

namespace DirectLDAP.Controllers
{
    public class HomeController : Controller
    {
        private readonly LoginService _loginService;

        public HomeController()
        {
            _loginService = new LoginService();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (_loginService.IsValid(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, false);
                return RedirectToAction("Authenticated");    
            }

            TempData["Message"] = "Failed to login";
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Authenticated");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Authenticated()
        {
            var user = _loginService.FindUserById(User.Identity.Name);
            return View(user);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index");
        }
    }
}