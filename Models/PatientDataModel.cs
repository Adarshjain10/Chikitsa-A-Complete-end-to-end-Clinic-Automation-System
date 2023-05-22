using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem.Models
{
    public class PatientDataModel
    {
        public int PatientID { get; set; }
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0:dd/MM/yyyy")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Please Select the Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please Enter Your Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter Your Contact Number")]
        [StringLength(10, ErrorMessage = "Contact Number Invalid", MinimumLength = 10)]
        public string ContactNo { get; set; }

        public string EmgContactName { get; set; }
        public string EmgContactNo { get; set; }
        public string History { get; set; }
        public string EmailID { get; set; }
        public int Age { get; set; }
        public List<PatientDataModel> ListPatient { get; set; }

    }
}