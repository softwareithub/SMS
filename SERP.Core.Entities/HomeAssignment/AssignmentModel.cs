using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.HomeAssignment
{
    [Table("AssignMaster")]
    public class AssignmentModel:Base<int>
    {
        [Required(ErrorMessage ="Please select Course")]
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Please select Subject")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Please select Batch")]
        public int BatchId { get; set; }
        public int TeacherId { get; set; } = default;
        public string AssignmentName { get; set; } = string.Empty;
        public string Assignment { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="AssignmentDate is required")]
        public DateTime AssignmentPublishDate { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Submission Date is required")]
        public DateTime SubmissionDate { get; set; }

        public string PDFPath { get; set; } = string.Empty;
        public int SessionId { get; set; }
    }
}
