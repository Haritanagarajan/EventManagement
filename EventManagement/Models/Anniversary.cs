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
    
    public partial class Anniversary
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Anniversary()
        {
            this.AddtoCarts = new HashSet<AddtoCart>();
        }
    
        public int id { get; set; }
        public Nullable<int> anniuserid { get; set; }
        public Nullable<int> anniid { get; set; }
        public Nullable<int> annidecorations { get; set; }
        public Nullable<int> annitheme { get; set; }
        public Nullable<int> annichairs { get; set; }
        public Nullable<int> annitables { get; set; }
        public Nullable<int> annihallcapacity { get; set; }
        public Nullable<System.DateTime> annidatetime { get; set; }
        public Nullable<int> annicakes { get; set; }
        public Nullable<int> annilocation { get; set; }
        public Nullable<long> annieventcost { get; set; }
        public bool annibeverages { get; set; }
        public bool anniPhotography { get; set; }
        public bool anniStyling { get; set; }
        public bool anniHospitality { get; set; }
        public Nullable<System.TimeSpan> annitime { get; set; }
    
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
