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

    public partial class Anniversary
    {
        public int id { get; set; }
        public Nullable<int> anniuserid { get; set; }
        public Nullable<int> anniid { get; set; }

        public Nullable<int> annidecorations { get; set; }


        public Nullable<int> annitheme { get; set; }



        [Required(ErrorMessage = "Choose  chairs")]
        [Range(0, 500, ErrorMessage = "The number of chairs must be between 0 and 500.")]
        public Nullable<int> annichairs { get; set; }




        [Required(ErrorMessage = "Choose  Tables")]
        [Range(0, 500, ErrorMessage = "The number of Tables must be between 0 and 100.")]
        public Nullable<int> annitables { get; set; }


        public Nullable<int> annihallcapacity { get; set; }

        [Required(ErrorMessage = "DateTime is Required")]
        public Nullable<System.DateTime> annidatetime { get; set; }


        public Nullable<int> annicakes { get; set; }


        public Nullable<int> annilocation { get; set; }


        public Nullable<long> annieventcost { get; set; }
        public bool annibeverages { get; set; }
        public bool anniPhotography { get; set; }
        public bool anniStyling { get; set; }
        public bool anniHospitality { get; set; }

        public virtual caketable caketable { get; set; }
        public virtual decorationtable decorationtable { get; set; }
        public virtual EventName EventName { get; set; }
        public virtual locationtable locationtable { get; set; }
        public virtual themetable themetable { get; set; }
        public virtual Usertable Usertable { get; set; }
    }
}
