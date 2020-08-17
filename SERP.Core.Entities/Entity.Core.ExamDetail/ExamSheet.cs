using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.ExamDetail
{
    [Table("ExamSheet",Schema = "Exam")]
    public class ExamSheet:Base<int>
    {
        [Required(ErrorMessage ="Please select Exam")]
        public int ExamId { get; set; }
        [Required(ErrorMessage = "Please Course")]
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Please Batch")]
        public int BatchId { get; set; }
        [Required(ErrorMessage = "Please Subject")]
        public int SubjectId { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Exam Date is required.")]
        public DateTime ExamDate { get; set; }
        [Required(ErrorMessage = "Please Start Time")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
        [Required(ErrorMessage = "Please End Time")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
        [Required(ErrorMessage = "Please Inveligitator")]
        public int InveligitatorId { get; set; } = default(int);
        public int MaxMark { get; set; } = 100;
        public int PassMark { get; set; } = 35;
        public int SessionId { get; set; }
    }
}
