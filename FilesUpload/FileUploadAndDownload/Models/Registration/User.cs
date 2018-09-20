using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileUploadAndDownload.Models.Registration
{
    public partial class User
    {
        //First Name validation

        [Display(Name ="First Name")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Please enter ur First Name")]
        public string FirstName { get; set; }

        //Last Name validation

        [Display(Name ="Last Name")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Please enter ur Last Name")]
        public string LastName { get; set; }

        //Email Id validation

        [Display(Name ="Email Id")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Please enter ur Email Id")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        //Date of Birth validation

        [Display(Name ="Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode =true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime DateOfBirth { get; set; }

        //Password Validation

        [Required(AllowEmptyStrings =false, ErrorMessage ="Please enter a password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage ="Password should be minimum of 6 charecters")]
        public string Password { get; set; }


    }
}