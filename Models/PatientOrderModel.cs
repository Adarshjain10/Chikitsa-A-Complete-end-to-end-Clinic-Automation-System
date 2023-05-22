using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Models
{
    public class PatientOrderModel
    {
        public int PatientOrderID { get; set; }

        [Required(ErrorMessage = "Please Select A Drug ")]
        public int DrugID { get; set; }

        [Required(ErrorMessage = "Please Enter A Quantity ")]
        [Range(1,Int32.MaxValue,ErrorMessage = " ")]
        public int Quantity { get; set; }
        public int? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public int PatientID { get; set; }
        public int SalespersonID { get; set; }

        public string PatientName { get; set; }
        public string DrugName { get; set; }
        public int QuantityAvailable { get; set; }
        public string QuantityType { get; set; }
        public List<SelectListItem> ListDrug { get; set; }
        public List<PatientOrderModel> ListOrder { get; set; }
    }
}