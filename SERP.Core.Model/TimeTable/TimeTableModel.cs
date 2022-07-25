using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.TimeTable
{
    public class TimeTableModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string EmployeeDetail { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public string DayName { get; set; }
        public int DayId { get; set; }
        public string FromT { get; set; }
        public string ToT { get; set; }
    }
}
