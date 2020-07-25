using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.ExamDetail
{
    [Table("StudentMarkAllocation",Schema = "Exam")]
    public class StudentMarkAllocation :Base<int>
    {
        public int ExamId { get; set; } = default;
        public int StudentId { get; set; } = default;
        public int SubjectId { get; set; } = default;
        public decimal AssignedMark { get; set; } = default;
        public int GradeId { get; set; } = default;
        public decimal LabMarks { get; set; } = default;
    }
}
