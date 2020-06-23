using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("AcademicCalender", Schema ="Master")]
    public class AcademicCalender :Base<int>
    {
        public int AcademicId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
