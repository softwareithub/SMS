using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("TimeTableMaster", Schema = "Transactionsch")]
    public class TimeTableMasterModel: Base<int>
    {
        [Required(ErrorMessage ="Please Select course")]
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Please Select Batch")]
        public int BatchId { get; set; }
        [Required(ErrorMessage = "Please enter time table name")]
        public string Name { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Please enter Periods")]
        public int Periods { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Please enter Days")]
        public int [] Days { get; set; }
        public ICollection<TimeTableAssignSubjTeacherModel> TimeTableAssignSubjTeacherModels { get; set; }
    }
}
