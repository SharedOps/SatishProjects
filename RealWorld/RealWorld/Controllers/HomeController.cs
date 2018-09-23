using RealWorld.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RealWorld.Models.EntityManager;
using RealWorld.Models.ViewModel;

namespace RealWorld.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Welcome()
        {
            return View();
        }

        //Means that allow only admin users to access the “AdminOnly” page

        [AuthorizeRoles("Admin")]
        public ActionResult AdminOnly()
        {
            return View();
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }


        //calls the GetUserDataView() method by passing in the loginName as the parameter
        //and return the result in the Partial View.
        [AuthorizeRoles("Admin")]
        public ActionResult ManageUserPartial()
        {
            if(User.Identity.IsAuthenticated)
            {
                string loginName = User.Identity.Name;
                UserManager UM = new UserManager();
                UserDataView UDV = UM.GetUserDataView(loginName);
                return PartialView(UDV);
            }
            return View();
        }
    }
}