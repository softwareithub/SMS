using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("EmployeeAttendence", Schema= "HR")]
    public class EmployeeAttendenceModel :Base<int>
    {
        public int EmployeeId { get; set; } = default;

        [Display(Name="Attendence Date", Prompt ="Please select Date")]
        [Required(ErrorMessage ="Attendence Date is required")]
        [DataType(DataType.Date)]
        public DateTime AttendenceDate { get; set; }
        public string AttendenceType { get; set; } = string.Empty;
        public int SessionId { get; set; }
    }
}
