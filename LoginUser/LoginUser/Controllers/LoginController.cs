using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoginUser.Models;

namespace LoginUser.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(User userModel)
        {
            using (LoginDataBaseEntities db = new LoginDataBaseEntities())
            {
                var userDetails = db.Users.Where(x => x.UserName == userModel.UserName && x.Password == userModel.Password).FirstOrDefault();
                if(userDetails==null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password";
                    return View("Index", userModel);
                }
                else
                {
                    Session["userId"] = userModel.UserId;
                    Session["userName"] = userModel.UserName;
                    return RedirectToAction("Index", "Home");
                }
            }

        }
        public ActionResult LogOut()
        {
            int userId = (int)Session["userId"];
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}