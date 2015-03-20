using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ADFS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var email = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            return View();
        }

        public ActionResult SignOut()
        {
            WSFederationAuthenticationModule.FederatedSignOut(new Uri("https://192.168.3.253/adfs/ls/"),
            new Uri("https://localhost:44303/"));

            return new EmptyResult();
        }
    }
}