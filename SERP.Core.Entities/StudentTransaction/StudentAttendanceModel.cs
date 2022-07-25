using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Entities.StudentTransaction
{
    public class StudentAttendanceModel
    {
        public IEnumerable<AttendanceModel> AttendanceModels { get; set; }
    }
    public class AttendanceModel
    {
        public string AttendanceDate { get; set; }
        public string Type { get; set; }
        public string DayName { get; set; }
        public int  PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int  OtherHolidayCount { get; set; }
        public string PresentPerc { get; set; }
        public string AbsentPerc { get; set; }
    }
}
