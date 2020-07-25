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
        public int TimeTableDayId { get; set; } = default;
        public string PeriodName { get; set; } = string.Empty;
        public int SubjecId { get; set; } = default;
        public int TeacherId { get; set; } = default;
        public TimeSpan FromTime { get; set; } = default;
        public TimeSpan ToTime { get; set; } = default;

        public string WeekDays { get; set; } = string.Empty;
        public TimeTableMasterModel TimeTableMasterModel { get; set; }
    }
}
