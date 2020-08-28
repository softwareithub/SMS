using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.TransactionViewModel
{
    public class PeriodVm
    {
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeImage { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public string SubjectName { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}
