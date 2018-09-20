using FileUploadAndDownload.Models.Registration;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Helpers;
using System.Web.Mvc;

namespace FileUploadAndDownload.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude ="IsEmailVerified,ActivationCode")] tblUser user)
        {
            bool status = false;
            string message = "";
            // Model Validation

            if(ModelState.IsValid)
            {
                #region Email already exists

                var isExist = IsEmailExist(user.EmailId);
                if(isExist)
                {
                    ModelState.AddModelError("EmailExists", "Email already exits");
                    return View(user);
                }

                #endregion

                #region Generate Actiation Code
                
                user.ActivationCode = Guid.NewGuid();

                #endregion

                #region Password Hashing
                user.Password = Crypto.Hash(user.Password);
                #endregion
                user.IsEmailVerified = false;

                #region Save to DataBase
                using (dbFilesEntities db = new dbFilesEntities())
                {
                    db.tblUsers.Add(user);
                    db.SaveChanges();

                    //Sending an email to user

                    SendVerificationLinkEmail(user.EmailId, user.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link has been sent to your email id: "+ user.EmailId;
                    status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = status;
            return View(user);
        }

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (dbFilesEntities dc = new dbFilesEntities())
            {
                var v = dc.tblUsers.Where(a => a.EmailId == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("saisatish.mundru@gmail.com", "Sai Satish");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "omsairammm@9918";
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 465,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }

    
}