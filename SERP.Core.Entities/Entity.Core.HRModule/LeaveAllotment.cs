using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("LeaveAllotment")]
    public class LeaveAllotment:Base<int>
    {
        [Required(ErrorMessage ="Select Leave Type.")]
        public int LeaveId { get; set; }
        [Required(ErrorMessage ="Select Designation")]
        public int DesignationId { get; set; }
        [Required(ErrorMessage ="Assign Leave per month")]
        public int LeavePerMonth { get; set; }
    }
}
