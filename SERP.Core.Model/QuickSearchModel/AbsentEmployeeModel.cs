using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.QuickSearchModel
{
    public class AbsentEmployeeModel
    {
        public string DayName { get; set; }
        public string PeriodName { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public string SubjectName { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string Photo { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        
    }
}
