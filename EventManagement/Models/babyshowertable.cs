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
    
    public partial class babyshowertable
    {
        public int id { get; set; }
        public Nullable<int> babyshoweruserid { get; set; }
        public Nullable<int> babyshowerid { get; set; }
        public Nullable<int> babyshowerdecorations { get; set; }
        public Nullable<int> babyshowertheme { get; set; }
        public Nullable<int> babyshowerchairs { get; set; }
        public Nullable<int> babyshowertables { get; set; }
        public Nullable<int> babyshowerhallcapacity { get; set; }
        public Nullable<System.DateTime> babyshowerdatetime { get; set; }
        public Nullable<int> babyshowercakes { get; set; }
        public Nullable<int> babyshowerlocation { get; set; }
        public Nullable<long> babyshowereventcost { get; set; }
        public bool babyshowerbeverages { get; set; }
    
        public virtual Usertable Usertable { get; set; }
        public virtual EventName EventName { get; set; }
        public virtual decorationtable decorationtable { get; set; }
        public virtual themetable themetable { get; set; }
        public virtual caketable caketable { get; set; }
        public virtual locationtable locationtable { get; set; }
    }
}
