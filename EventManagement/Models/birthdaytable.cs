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
    
    public partial class birthdaytable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public birthdaytable()
        {
            this.AddtoCarts = new HashSet<AddtoCart>();
        }
    
        public int id { get; set; }
        public Nullable<int> bdayuserid { get; set; }
        public Nullable<int> bdayid { get; set; }
        public Nullable<int> bdaydecorations { get; set; }
        public Nullable<int> bdaytheme { get; set; }
        public Nullable<int> bdaychairs { get; set; }
        public Nullable<int> bdaytables { get; set; }
        public Nullable<int> bdayhallcapacity { get; set; }
        public Nullable<System.DateTime> bdaydatetime { get; set; }
        public Nullable<int> bdaycakes { get; set; }
        public Nullable<int> bdaylocation { get; set; }
        public Nullable<long> bdayeventcost { get; set; }
        public bool bdaybeverages { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddtoCart> AddtoCarts { get; set; }
        public virtual caketable caketable { get; set; }
        public virtual decorationtable decorationtable { get; set; }
        public virtual EventName EventName { get; set; }
        public virtual locationtable locationtable { get; set; }
        public virtual themetable themetable { get; set; }
        public virtual Usertable Usertable { get; set; }
    }
}
