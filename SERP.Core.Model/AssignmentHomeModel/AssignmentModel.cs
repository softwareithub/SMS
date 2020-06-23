using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.AssignmentHomeModel
{
    public class AssignmentModel
    {
        public int AssignmentId { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string SubjectName { get; set; }
        public string AssignmentName { get; set; }
        public string Assignment { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string  PDFPath { get; set; }
    }
}
