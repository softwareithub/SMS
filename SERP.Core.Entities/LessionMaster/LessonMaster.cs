using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.LessionMaster
{
    [Table("LessonMaster", Schema ="Master")]
    public class LessonMaster:Base<int>
    {
        [Required(ErrorMessage ="Please select subject")]
        public int SubjectId { get; set; } = default;

        [Required(ErrorMessage = "Please enter lesson name")]
        public string LessonName { get; set; }

        [Required(ErrorMessage = "Please enter start date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }        
        [DataType(DataType.Date)]

        [Required(ErrorMessage = "Please enter end date")]
        public DateTime? EndDate { get; set; }
        [DataType(DataType.Date)]

        [Required(ErrorMessage = "Please enter class test date")]
        public DateTime? ClassTestDate { get; set; }
        public int SessionId { get; set; }
    }
}
