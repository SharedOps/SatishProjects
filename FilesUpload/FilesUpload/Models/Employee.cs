using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilesUpload.Models
{
    public class Employee
    {
        public int Id
        {
            get;
            set;
        }
        public string FirstName
        {
            get;
            set;
        }
        public string LastName
        {
            get;
            set;
        }
        public string Skills
        {
            get;
            set;
        }
        public string EmailID
        {
            get;
            set;
        }
        public string ContactNo
        {
            get;
            set;
        }
        public string Position
        {
            get;
            set;
        }
        public HttpPostedFileBase Resume
        {
            get;
            set;
        }
    }
}