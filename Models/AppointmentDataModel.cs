using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Models
{
    public class AppointmentDataModel
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Please Select the Physician Name")]
        public int PhysicianID { get; set; }

        [Required(ErrorMessage = "Please Enter the Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please Enter the Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please Enter the Appointment Date")]
        public DateTime AppointmentDate { get; set; }
        public string AppointmentStatus { get; set; }

        public string PatientName { get; set; }
        public string PatientGender { get; set; }

        public string PhysicianName { get; set; }
        public List<SelectListItem> ListPhysician { get; set; }

        [Required(ErrorMessage = "Please Select the Specialization")]
        public int SpecializationID { get; set; }
        public List<SelectListItem> ListSpecialization { get; set; }
        
        public List<AppointmentDataModel> ListData { get; set; }
    }
}