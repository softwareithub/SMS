using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("BatchMaster", Schema = "Master")]
    public class BatchMaster : Base<int>
    {
        [Required(ErrorMessage = "Please enter batch name"), MaxLength(150)]        
        public string BatchName { get; set; }

        [Required(ErrorMessage = "Please enter batch code")]
        public string BatchCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select course")]
        [Column("CourseMasterId")]
        public int? CourseId { get; set; }
    }
}
