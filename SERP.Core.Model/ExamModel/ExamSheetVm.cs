using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.ExamModel
{
    public class ExamSheetVm
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public DateTime ExamDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string TeacherName { get; set; }
        public int MaxMark { get; set; }
        public int PassMark { get; set; }
        public string  ExamDateStr { get; set; }
        public string ExamFromTime { get; set; }
        public string ExamToTime { get; set; }
    }
}
