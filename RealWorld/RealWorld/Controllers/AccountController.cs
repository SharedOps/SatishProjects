using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealWorld.Models.EntityManager;
using RealWorld.Models.ViewModel;
using System.Web.Security;

namespace RealWorld.Controllers
{
    public class AccountController : Controller
    {
        
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserSignUpView USV)
        {
            if(ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                if(!UM.IsLoginNameExist(USV.LoginName))
                {
                    UM.AddUserAccount(USV);
                    FormsAuthentication.SetAuthCookie(USV.FirstName, false);
                    return RedirectToAction("Welcome", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login Name already taken");
                }

            }
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLoginView ULV, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                string password = UM.GetUserPassword(ULV.LoginName);

                if(string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "The user login or password provided is Incorrect");
                }
                else
                {
                    if(ULV.Password.Equals(password))
                    {
                        FormsAuthentication.SetAuthCookie(ULV.LoginName, false);
                        return RedirectToAction("Welcome", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The password provided is Incorrect");
                    }
                }
            }

            // If we got this far, something failed, redisplay form

            return View(ULV);
        }
        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}