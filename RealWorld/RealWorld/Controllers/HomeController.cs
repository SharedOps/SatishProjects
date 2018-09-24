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
        public ActionResult ManageUserPartial(string status = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                string loginName = User.Identity.Name;
                UserManager UM = new UserManager();
                UserDataView UDV = UM.GetUserDataView(loginName);
                string message = string.Empty;
                if (status.Equals("update"))
                    message = "Update Successful";
                else if (status.Equals("delete"))
                    message = "Delete Successful";
                ViewBag.Message = message;
                return PartialView(UDV);
            }
                return RedirectToAction("Index", "Home");
        }

        //This method is responsible for collecting data that is sent from the View for update.
        [AuthorizeRoles("Admin")]
        public ActionResult UpdateUserData(int userID, string loginName, string password, string firstName, string lastName, string gender, int roleID = 0)
        {
            UserProfileView UPV = new UserProfileView
            {
                SYSUserID = userID,
                LoginName = loginName,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Gender = gender
            };
            if (roleID>0)
            {
                UPV.LOOKUPRoleID = roleID;
            }
            UserManager UM = new UserManager();
            UM.UpdateUserAccount(UPV);

            return Json(new { success = true });
        }
    }
}