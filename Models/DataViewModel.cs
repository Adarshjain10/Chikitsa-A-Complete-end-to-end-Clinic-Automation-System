using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Models
{
    public class DataViewModel
    {
        
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please Enter the Role")]
        public int RoleID { get; set; }

        [Required(ErrorMessage = "Please Enter Your User Name")]
        [StringLength(15, ErrorMessage = "Username must be of atleast 5 characters and maximum 15 characters", MinimumLength = 5)]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Please Enter Your Password")]
        //[StringLength(12, ErrorMessage = "Password must be of atleast 4 characters", MinimumLength = 4)]
        //public string Password { get; set; }

        //[Required(ErrorMessage = "Please Enter Your Password")]
        //[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password does not match")]
        //public string ConfirmPassword { get; set; }

        public Boolean IsActive { get; set; }
        public Boolean IsLocked { get; set; }
        public DateTime LastLogDate { get; set; }

        [Required(ErrorMessage = "Please Enter Your E-mail ID")]
        [EmailAddress(ErrorMessage = "Invalid E-mail Address")]
        public string EmailID { get; set; }
        public Boolean IsEmailVerified { get; set; }

        public string RoleName { get; set; }
        public List<SelectListItem> ListRole { get; set; }
    }
}