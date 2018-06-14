using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoConnect.Models;
using System.Data;

namespace GeoConnect.Controllers
{
    public class EmployeeController : Controller
    {
        DAL.EmpCRUD db = new DAL.EmpCRUD();
        // GET: Employee
        public ActionResult Index()
        {
            DataSet ds = db.Read_Employee();
            ViewBag.emp = ds.Tables[0];
            return View();
        }

        public ActionResult Add_Employee()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Add_Employee(FormCollection frmCollection)
        {
            Employee emp = new Employee()
            {
                Name = frmCollection["Name"],
                Address = frmCollection["Address"],
                City = frmCollection["City"],
                PinCode = frmCollection["PinCode"],
                Designation = frmCollection["Designation"]
            };
            db.Add_Employee(emp);
            TempData["msg"] = "Records Inserted";
            return View();
        }

        public ActionResult Update_Employee(int id)
        {
            DataSet ds = db.Read_Employee_Id(id);
            ViewBag.emprec = ds.Tables[0];
            return View();
        }
        [HttpPost,ActionName("Update_Employee")]
        public ActionResult Update_Employee(int id, FormCollection frmCollection)
        {
            Employee emp = new Employee()
            {
                Emp_Id = id,
                Name = frmCollection["Name"],
                Address = frmCollection["Address"],
                City = frmCollection["City"],
                PinCode = frmCollection["PinCode"],
                Designation = frmCollection["Designation"]
            };
            db.Update_Employee(emp);
            return RedirectToAction("Index");
        }

        public ActionResult Delete_Employee(int id)
        {
            db.Delete_Employee(id);
            return RedirectToAction("Index");
        }
    }
}