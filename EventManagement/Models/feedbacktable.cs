//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class feedbacktable
    {
        public int id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Experiencetype { get; set; }
        public string Comments { get; set; }
        public string UserName { get; set; }
        public string Useremail { get; set; }
    }
}
