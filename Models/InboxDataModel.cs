using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem.Models
{
    public class InboxDataModel
    {
        public int MessageID { get; set; }
        public string FromEmailID { get; set; }
        public string ToEmailID { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "Please Enter the Message")]
        public string MessageDetail { get; set; }
        public DateTime MessageDate { get; set; }
        public int ReplyID { get; set; }
        public Boolean IsRead { get; set; }

        public string PatientName { get; set; }
        public string PhysicianName { get; set; }
        public string SalespersonName { get; set; }
        public string SupplierName { get; set; }
        public List<InboxDataModel> ListData { get; set; }
    }
}