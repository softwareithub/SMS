using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.DashBoardModel
{
    public class TeacherAbsentModel
    {
        public string EmployeeName { get; set; }
        public string  EmployeeCode { get; set; }
        public string Photo { get; set; }
        public string PeriodName { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public int CourseId { get; set; }
        public int BatchId { get; set; }
        public int SubjectId { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string AttendenceType { get; set; }
        public string SubjectName { get; set; }


    }
}
