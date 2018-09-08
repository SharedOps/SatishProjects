using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using WebApiImageUpload.Models.ImageModel;
using System.Text;

namespace WebApiImageUpload.Controllers.API
{
    public class ImageApiController : ApiController
    {
        [HttpPost]
        public void AddStudent(Student newStudent)
        {
           
                string spName = Models.Constants.GeoStudentsConts.spAddStudent;
            Dictionary<string, SqlParameter> cmdParams = new Dictionary<string, SqlParameter>
            {
                ["@FirstName"] = new SqlParameter("@FirstName", newStudent.FirstName),
                ["@LastName"] = new SqlParameter("@LastName", newStudent.LastName),
                ["@Company"] = new SqlParameter("@Company", newStudent.Company),
                ["@Image"] = new SqlParameter("@Image", newStudent.Image.ToString())
            };
            DataAccessLayer.DatabaseAccess.DALExecuteCommand(spName, cmdParams);
            
            

        }

    }
}
