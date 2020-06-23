using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.ExamDetail
{
    [Table("StudentMarkAllocation",Schema = "Exam")]
    public class StudentMarkAllocation :Base<int>
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public decimal AssignedMark { get; set; }
        public int GradeId { get; set; }
        public decimal LabMarks { get; set; }
    }
}
