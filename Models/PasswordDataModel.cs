using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem.Models
{
    public class PasswordDataModel
    {
        [Required(ErrorMessage = "Please Enter Your Password")]
        [StringLength(12, ErrorMessage = "Password must be of atleast 4 characters", MinimumLength = 4)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Your Password")]
        [StringLength(12, ErrorMessage = "Password must be of atleast 4 characters", MinimumLength = 4)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Your Password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Password does not match")]
        public string ConfirmNewPassword { get; set; }
    }
}