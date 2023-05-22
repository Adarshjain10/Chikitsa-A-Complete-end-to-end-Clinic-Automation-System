using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem.Models
{
    public class SupplierDataModel
    {
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter the Company Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Please Enter the Company Name")]
        public string CompanyAddress { get; set; }

        [Required(ErrorMessage = "Please Enter Your Working Status")]
        public string CurrentStatus { get; set; }

        public int UserID { get; set; }

        public string EmailID { get; set; }


        public List<SupplierDataModel> ListSupplier { get; set; }
    }
}