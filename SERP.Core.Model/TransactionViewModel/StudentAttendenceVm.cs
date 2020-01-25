using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.TransactionViewModel
{
    public class StudentAttendenceVm 
    {
        public int AttendenceId { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string RollCode { get; set; }
        public string StudentImage { get; set; }
        public DateTime AttendenceDate { get; set; }
        public string AttendenceType { get; set; }
        public int StudentId { get; set; }
        public string AttendenceMarkType { get; set; }
    }
}
