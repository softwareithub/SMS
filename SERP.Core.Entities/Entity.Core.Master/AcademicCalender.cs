using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("AcademicCalender", Schema ="Master")]
    public class AcademicCalender :Base<int>
    {
        public int AcademicId { get; set; } = default;
        public string EventName { get; set; } = string.Empty;
        public string EventDescription { get; set; } = string.Empty;
        public DateTime FromDate { get; set; } = DateTime.Now.Date;
        public DateTime ToDate { get; set; } = DateTime.Now.Date;
        public int IsHoliday { get; set; } = 0;
    }
}
