
using GeoStudent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoStudent.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //Uploading or inserting file
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                StudentEntities db = new StudentEntities();
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/Images/" + ImageName);
                file.SaveAs(physicalPath);
                tblStudent student = new tblStudent
                {
                    FirstName = Request.Form["firstname"],
                    LastName = Request.Form["lastname"],
                    ImageUrl = ImageName
                };
                db.tblStudents.Add(student);
                db.SaveChanges();

            }
            return RedirectToAction("../home/DisplayImage/");
        }


        //Displaying the image with data
        public ActionResult DisplayImage()
        {
            return View();
        }
    }
}