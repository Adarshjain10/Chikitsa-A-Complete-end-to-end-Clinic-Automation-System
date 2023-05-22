using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem.Models
{
    public class SalesOrderModel
    {
        public int SalespersonOrderID { get; set; }

        [Required(ErrorMessage = "Please Enter the Drug Name ")]
        public string DrugName { get; set; }

        [Required(ErrorMessage = "Please Enter A Quantity ")]
        public int Quantity { get; set; }
        public int? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public int SalespersonID { get; set; }
        public int SupplierID { get; set; }

        public string SupplierName { get; set; }
        public string SupplierCompanyName { get; set; }
        //public string SupplierCompanyAddress{ get; set; }
        public List<SalesOrderModel> ListOrder { get; set; }

        //public List<SalesOrderModel> ListSupplier { get; set; }
    }
}