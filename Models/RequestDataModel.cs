using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Models
{
    public class RequestDataModel
    {
        public int RequestID { get; set; }

        [Required(ErrorMessage ="Please Enter Your First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Your E-mail Address")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Please Select Your Role")]
        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public string Status { get; set; }
        public List<SelectListItem> ListRole { get; set; }

        public List<RequestDataModel> ListRequest { get; set; }
    }
}