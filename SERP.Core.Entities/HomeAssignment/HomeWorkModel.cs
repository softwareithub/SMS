using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.HomeAssignment
{
    [Table("HomeWork", Schema = "Master")]
    public class HomeWorkModel : Base<int>
    {
        [Required(ErrorMessage = "Home Work name is required.")]
        [Display(Prompt = "Please Enter Home Work")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select Batch")]
        [Display(Prompt = "Please select Batch")]
        public int BatchId { get; set; }
        [Required(ErrorMessage ="Please select Course")]
        [Display(Prompt ="Select Course Name")]
        public int CourseId { get; set; } 
        [Required(ErrorMessage ="Please select subject name")]
        public int SubjectId { get; set; }
        public int? EmployeeId { get; set; } = default;
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Home work  Date is required.")]
        [Display(Prompt = "Please Enter Home Work  Date")]
        public DateTime? HomeWorkDate { get; set; } = default;
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Home work submmison Date is required.")]
        [Display(Prompt ="Please Enter Home Work Submission Date")]
        public DateTime? HomeWorkSubmissionDate { get; set; } = default;
        public string HomeWork { get; set; } = string.Empty;
        public string PDFPath { get; set; } = string.Empty;
        public int SessionId { get; set; }
    }
}
