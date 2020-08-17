using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.ExamDetail
{
    [Table("GradeMaster", Schema = "Exam")]
    public class GradeMaster : Base<int>
    {
        [Required(ErrorMessage ="Maximum marks is required.")]
        public int MaximumMarks { get; set; }

        [Required(ErrorMessage = "From marks is required.")]
        public int FromMarks { get; set; }
        [Required(ErrorMessage ="To marks is required.")]
        public int ToMarks { get; set; }
        [Required(ErrorMessage ="Grade is required.")]
        public string GradeName { get; set; }
        public string ResultType { get; set; } = string.Empty;
        public int SessionId { get; set; }
    }
}
