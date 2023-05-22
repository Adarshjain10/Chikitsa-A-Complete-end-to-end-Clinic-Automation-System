using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Models
{
    public class PhysicianDataModel
    {
        public int PhysicianID { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Select the Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please Enter the Total Experience")]
        public int? TotalExperience { get; set; }

        [Required(ErrorMessage = "Please Enter Your Education")]
        public string Education { get; set; }

        [Required(ErrorMessage = "Please Enter Your Working Status")]
        public string CurrentStatus { get; set; }

        [Required(ErrorMessage = "Please Select Specialization")]
        public int? SpecializationID { get; set; }
        public string SpecializationName { get; set; }
        public string EmailID { get; set; }

        public int UserID { get; set; }
        public List<SelectListItem> ListSpecialization { get; set; }

        public List<PhysicianDataModel> ListPhysician { get; set; }
    }
}