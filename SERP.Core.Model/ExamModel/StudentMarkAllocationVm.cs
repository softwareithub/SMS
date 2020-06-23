using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.ExamModel
{
    public class StudentMarkAllocationVm
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string RollCode { get; set; }
        public int Subject { get; set; }
        public string SubjectName { get; set; }
        public decimal AssignedMarks { get; set; }
        public decimal LabMarks { get; set; }
        public int MaxMarks { get; set; }
        public int PassMark { get; set; }
        public string Grade { get; set; }
        public decimal Percentage { get; set; }
    }
}
