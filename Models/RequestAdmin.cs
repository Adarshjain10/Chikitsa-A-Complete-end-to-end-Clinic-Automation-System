//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClinicalAutomationSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RequestAdmin
    {
        public int RequestID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public string Status { get; set; }
    
        public virtual RoleDetail RoleDetail { get; set; }
    }
}
