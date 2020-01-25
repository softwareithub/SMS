using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("TimeTableAssign", Schema = "Transactionsch")]
    public class TimeTableAssignSubjTeacherModel :Base<int>
    {
        [ForeignKey("TimeTableDayId")]
        public int TimeTableDayId { get; set; }
        public string PeriodName { get; set; }
        public int SubjecId { get; set; }
        public int TeacherId { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }

        public string WeekDays { get; set; }
        public TimeTableMasterModel TimeTableMasterModel { get; set; }
    }
}
