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

    public partial class eventname
    {
        [Required(ErrorMessage = "eventid is required.")]
        public Nullable<int> eventid { get; set; }


        [Required(ErrorMessage = "eventnameid is required.")]
        public int eventnameid { get; set; }


        [Required(ErrorMessage = "Userid is required.")]
        public Nullable<int> TUserid { get; set; }


        [Required(ErrorMessage = "eventdate is required.")]
        public Nullable<int> eventdate { get; set; }


        [Required(ErrorMessage = "eventtime is required.")]
        public Nullable<int> eventtime { get; set; }


        [Required(ErrorMessage = "eventhallcapacity must be a positive long.")]
        public Nullable<long> eventhallcapacity { get; set; }


        [Required(ErrorMessage = "eventcatering is required.")]
        public bool eventcatering { get; set; }


        [Required(ErrorMessage = "eventlocation is required.")]
        public Nullable<int> eventlocation { get; set; }


        [Required(ErrorMessage = "eventtheme is required.")]
        public Nullable<int> eventtheme { get; set; }


        [Required(ErrorMessage = "eventcost is required.")]
        public Nullable<long> eventcost { get; set; }
    

        public virtual datetable datetable { get; set; }
        public virtual eventstable eventstable { get; set; }
        public virtual timetable timetable { get; set; }
        public virtual locationtable locationtable { get; set; }
        public virtual themetable themetable { get; set; }
        public virtual UserTable UserTable { get; set; }
    }
}
