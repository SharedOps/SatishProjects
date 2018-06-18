using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistrationAndLogin.Models;
using System.Net.Mail;
using System.Net;

namespace RegistrationAndLogin.Controllers
{
    public class UserController : Controller
    {
        //Registration action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration Post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude ="IsEmailVerified,ActivationCode")] User user)
        {
            bool Status = false;
            string msg = "";
            //Model Validation
            if(ModelState.IsValid)
            {
                #region//Email if already exists
                var isExist = IsEmailExist(user.EmailId);
                if(isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email Already Exist");
                    return View(user);
                }
                #endregion

                #region Generate activation code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerified = false;

                #region save data to database
                using (RegAndLogDbEntities db = new RegAndLogDbEntities())
                {
                    db.Users.Add(user);
                    db.SaveChanges();

                    //send email to user
                    SendVerificationLinkEmail(user.EmailId, user.ActivationCode.ToString());

                    msg = "Registration successfully done. Account activation link " +
                       " has been sent to your email id:" + user.EmailId;
                    Status = true;


                }

                #endregion



            }
            else
            {
                msg = "Invalid Request";
            }

            ViewBag.Message = msg;
            ViewBag.Status = Status;
            return View(user);

        }

        //Verifying Email


        //Verifying Email Link


        //Login Action


        //Login Post Action

        //Logout


        [NonAction]
        public bool IsEmailExist(string emailId)
        {
            using (RegAndLogDbEntities db = new RegAndLogDbEntities())
            {
                var v = db.Users.Where(a => a.EmailId == emailId).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailId,string actvationCode)
        {
            var verifyUrl = "User/VerifyAccount/" + actvationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress("saisatish.mundru@gmail.com", "Sai Satish");
            var toEmail = new MailAddress(emailId);
            var fromEmailPassword = "********"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var msg = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(msg);
        }
    }
}