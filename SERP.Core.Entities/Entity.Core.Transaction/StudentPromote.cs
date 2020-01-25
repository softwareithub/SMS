using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("StudentPromote",Schema = "TransactionSch")]
    public class StudentPromote: Base<int>
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int BatchId { get; set; }
        [Required(ErrorMessage ="Promote Date is required")]
        public DateTime PromotionDate { get; set; }
    }
}
