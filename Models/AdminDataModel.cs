using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem.Models
{
    public class AdminDataModel
    {
        public int AdminID { get; set; }

        public int UserID { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Select the Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please Enter Your Address")]
        public string Address { get; set; }
    }
}