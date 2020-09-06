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
        public string LessonName { get; set; }        
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }        
        [DataType(DataType.Date)]        
        public DateTime? EndDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ClassTestDate { get; set; }
        public int SessionId { get; set; }
        [NotMapped]
        public int CourseId { get; set; }
    }
}
