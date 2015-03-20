using System.Web.Mvc;
using System.Web.Security;
using ActiveDirectoryMembershipProvider.Models;

namespace ActiveDirectoryMembershipProvider.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (Membership.ValidateUser(username, password))
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
            var membershipUser = (ActiveDirectoryMembershipUser)Membership.GetUser(User.Identity.Name);

            var user = new ActiveDirectoryUser
            {                
                Email = membershipUser.Email,
                GivenName = membershipUser.UserName
            };

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