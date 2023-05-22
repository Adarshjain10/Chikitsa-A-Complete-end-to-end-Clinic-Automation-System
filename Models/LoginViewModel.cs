using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem.Models
{
    public class LoginViewModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please Enter Your User Name")]
        [StringLength(15, ErrorMessage = "Username must be of atleast 5 characters and maximum 15 characters", MinimumLength = 5)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Password")]
        [StringLength(12, ErrorMessage = "Password must be of atleast 4 characters", MinimumLength = 4)]
        public string Password { get; set; }

        public string EmailID { get; set; }
    }
}