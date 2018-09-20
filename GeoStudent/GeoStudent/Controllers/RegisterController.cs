using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;

namespace GeoStudent.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Registeration
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            dbFilesEntities usersEntities = new dbFilesEntities();
            usersEntities.Users.Add(user);
            usersEntities.SaveChanges();
            string message = string.Empty;
            switch (user.UserId)
            {
                case -1:
                    message = "Username already exists.\\nPlease choose a different username.";
                    break;
                case -2:
                    message = "Supplied email address has already been used.";
                    break;
                default:
                    message = "Registration successful.\\nUser Id: " + user.UserId.ToString();
                    break;
            }
            ViewBag.Message = message;

            return View(user);
        }

        public ActionResult Activation()
        {
            ViewBag.Message = "Invalid Activation code.";
            if (RouteData.Values["id"] != null)
            {
                Guid activationCode = new Guid(RouteData.Values["id"].ToString());
                dbFilesEntities usersEntities = new dbFilesEntities();
                UserActivation userActivation = usersEntities.UserActivations.Where(p => p.ActivationCode == activationCode).FirstOrDefault();
                if (userActivation != null)
                {
                    usersEntities.UserActivations.Remove(userActivation);
                    usersEntities.SaveChanges();
                    ViewBag.Message = "Activation successful.";
                }
            }

            return View();
        }

        private void SendActivationEmail(User user)
        {
            Guid activationCode = Guid.NewGuid();
            dbFilesEntities usersEntities = new dbFilesEntities();
            usersEntities.UserActivations.Add(new UserActivation
            {
                UserId = user.UserId,
                ActivationCode = activationCode
            });
            usersEntities.SaveChanges();

            using (MailMessage mm = new MailMessage("saisatish.mundru@gmail.com", user.Email))
            {
                mm.Subject = "Account Activation";
                string body = "Hello " + user.Username + ",";
                body += "<br /><br />Please click the following link to activate your account";
                body += "<br /><a href = '" + string.Format("{0}://{1}/Register/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Click here to activate your account.</a>";
                body += "<br /><br />Thanks";
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    EnableSsl = true
                };
                NetworkCredential NetworkCred = new NetworkCredential("saisatish.mundru@gmail.com", "omsairammm@9918");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }
}