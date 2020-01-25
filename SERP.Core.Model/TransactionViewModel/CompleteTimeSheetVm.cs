using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.TransactionViewModel
{
    public class CompleteTimeSheetVm
    {
        public int MasterId { get; set; }
        public int AttendenceId { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string EmployeeName { get; set; }
        public string Photo { get; set; }
        public string Phone { get; set; }
        public string PeriodName { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public string SubjectName { get; set; }
        public string TimeTableDay { get; set; }
        public string TeacherAttendence { get; set; }
    }
}
