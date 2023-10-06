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
    using System.ComponentModel.DataAnnotations;

    public partial class caketable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public caketable()
        {
            this.birthdaytables = new HashSet<birthdaytable>();
            this.Reunions = new HashSet<Reunion>();
            this.Anniversaries = new HashSet<Anniversary>();
            this.babyshowertables = new HashSet<babyshowertable>();
            this.BachelorParties = new HashSet<BachelorParty>();
            this.CocktailParties = new HashSet<CocktailParty>();
            this.Weddings = new HashSet<Wedding>();
        }
    
        public int cakeid { get; set; }


        [Required(ErrorMessage ="Cake Name is Required")]
        public string cakesavailable { get; set; }


        public bool caked { get; set; }


        //[Required(ErrorMessage = "Cake Image is Required")]

        public byte[] cakeimage { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<birthdaytable> birthdaytables { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reunion> Reunions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Anniversary> Anniversaries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<babyshowertable> babyshowertables { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BachelorParty> BachelorParties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CocktailParty> CocktailParties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wedding> Weddings { get; set; }
    }
}
