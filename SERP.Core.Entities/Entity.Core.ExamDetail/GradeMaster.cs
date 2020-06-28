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
        [Required(ErrorMessage = "From Percentage is required.")]

        public int FromPercentage { get; set; }

        [Required(ErrorMessage ="To Percentage is required.")]

        public int ToPercentage { get; set; }
        [Required(ErrorMessage ="Grade is required.")]
        public string GradeName { get; set; }
    }
}
