using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.StudentTransaction
{
    [Table("StudentAttendence", Schema = "StudentTransaction")]
    public class StudentAttendenceModel :Base<int>
    {
        [Column("StudentId")]
        public int Student { get; set; }
        public DateTime AttendenceDate { get; set; }
        public string AttendenceType { get; set; }
        public string PeriodId { get; set; }
        public int SessionId { get; set; }
    }
}
