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
    
    public partial class BachelorParty
    {
        public int id { get; set; }
        public Nullable<int> bacheloruserid { get; set; }
        public Nullable<int> bachelorid { get; set; }
        public Nullable<int> bachelordecorations { get; set; }
        public Nullable<int> bachelortheme { get; set; }
        public Nullable<int> bachelorchairs { get; set; }
        public Nullable<int> bachelortables { get; set; }
        public Nullable<int> bachelorhallcapacity { get; set; }
        public Nullable<System.DateTime> bachelordatetime { get; set; }
        public Nullable<int> bachelorcakes { get; set; }
        public Nullable<int> bachelorlocation { get; set; }
        public Nullable<long> bacheloreventcost { get; set; }
        public bool bachelorbeverages { get; set; }
    
        public virtual Usertable Usertable { get; set; }
        public virtual EventName EventName { get; set; }
        public virtual decorationtable decorationtable { get; set; }
        public virtual themetable themetable { get; set; }
        public virtual caketable caketable { get; set; }
        public virtual locationtable locationtable { get; set; }
    }
}
