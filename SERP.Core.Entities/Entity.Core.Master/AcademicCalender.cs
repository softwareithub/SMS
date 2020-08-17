using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("AcademicCalender", Schema ="Master")]
    public class AcademicCalender :Base<int>
    {
        [Required(ErrorMessage ="Please select session")]
        public int AcademicId { get; set; } = default;

        [Required(ErrorMessage ="Please enter event name")]
        public string EventName { get; set; } = string.Empty;
        public string EventDescription { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter start date")]
        public DateTime FromDate { get; set; } = DateTime.Now.Date;
        [Required(ErrorMessage = "Please enter end date")]
        public DateTime ToDate { get; set; } = DateTime.Now.Date;
        public int IsHoliday { get; set; } = 0;
        public int SessionId { get; set; }
    }
}
