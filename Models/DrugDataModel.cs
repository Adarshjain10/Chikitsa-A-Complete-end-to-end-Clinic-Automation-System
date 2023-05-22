using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem.Models
{
    public class DrugDataModel
    {
        public int DrugID { get; set; }

        [Required(ErrorMessage = "Please provide the Drug Name")]
        public string DrugName { get; set; }

        [Required(ErrorMessage = "Please provide the Drug Manufacturer")]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Please provide the Drug Substitutions")]
        public string Substitutions { get; set; }

        [Required(ErrorMessage = "Please provide the Drug Uses")]
        public string Uses { get; set; }

        [Required(ErrorMessage = "Please provide the Side Effects")]
        public string SideEffects { get; set; }

        [Required(ErrorMessage = "Please provide the Non-Recommended Users")]
        public string NotRecommended { get; set; }

        public Boolean? IsDeleted { get; set; }

        public List<DrugDataModel> ListDrugs { get; set; }

        //Batch Table

        [Required(ErrorMessage = "Please provide the Manufacturing Date")]
        public DateTime MfgDate { get; set; }

        [Required(ErrorMessage = "Please provide the Expiry Date")]
        public DateTime ExpDate { get; set; }

        [Required(ErrorMessage = "Please provide the Quantity on Hand")]
        public int QOH { get; set; }

        [Required(ErrorMessage = "Please provide the Type of Quantity on Hand")]
        public string QOHType { get; set; }

        [Required(ErrorMessage = "Please provide the Price")]
        public double Price { get; set; }

        public double DiscountAmount { get; set; }
    }
}