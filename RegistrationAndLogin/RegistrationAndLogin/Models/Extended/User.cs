using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RegistrationAndLogin.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }

    public class UserMetaData
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "FirstName required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        public string LastName { get; set; }

        [Display(Name = "Email Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Id is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Required(AllowEmptyStrings =false,ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Password should be minimum 6 charecters")]
        public string Password { get; set; }


        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Both Password should match")]
        public string ConfirmPassword { get; set; }

    }
}