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

    public partial class UserTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserTable()
        {
            this.eventnames = new HashSet<eventname>();
        }

        [Key]
        public int TUserid { get; set; }


        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Username should accept only alphabetic characters.")]
        public string TUsername { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[@123]).*$", ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, and '@123'.")]
        public string TPassword { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string TEmail { get; set; }


        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Mobile number should be 10 digits.")]
        public Nullable<long> TMobile { get; set; }


 
        public Nullable<System.DateTime> LastLoginDate { get; set; }

        
        public Nullable<int> TRoleid { get; set; }


       
        public byte[] Tprofile { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eventname> eventnames { get; set; }
        public virtual RoleTable RoleTable { get; set; }
    }
}
