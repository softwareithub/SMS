using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SERP.Core.Model.TransactionViewModel
{
    public class EmployeeAttendenceVm
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage ="Please select Attendence Date")]
        [DataType(DataType.Date)]
        public DateTime AttendenceDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ToAttendenceDate { get; set; }
        public string AttendenceType { get; set; }
        [Required(ErrorMessage = "Please select Department")]
        public int DepartmentId { get; set; }
        public string LongLeaveType { get; set; }
        public string EmployeeName { get; set; }
        public string  EmployeeCode { get; set; }
        public string Image { get; set; }
    }
}
