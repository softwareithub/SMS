using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.AssignmentHomeModel
{
    public class HomeWorkModelVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string SubjectName { get; set; }
        public string EmployeeName { get; set; }
        public string HomeWork { get; set; }
        public string PDFPath { get; set; }
        public DateTime  PublishDate { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}
