using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FilesUpload.Models;

namespace FilesUpload.Controllers
{
    public class FileUploadController : Controller
    {
        // GET: FileUpload
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }
        //Post: Uploading the data
        [HttpPost]
        public ActionResult Upload(Employee employee)
        {
            using (EMPLOYEEDBEntities db = new EMPLOYEEDBEntities())
            {
                var candidates = new Candidate()
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Skills = employee.Skills,
                    EmailID = employee.EmailID,
                    ContactNo = employee.ContactNo,
                    Position = employee.Position,
                    Resume= SaveToPhysicalLocation(employee.Resume),
                    CreatedOn=DateTime.Now

                };
                db.Candidates.Add(candidates);
                db.SaveChanges();
            }
            return View(employee);
        }

        // Save Posted File in Physical path and return saved path to store in a database 

        private string SaveToPhysicalLocation(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Files"), fileName);
                file.SaveAs(path);
                return path;
            }
            return string.Empty;
        }
    }
}