using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.StudentPortal
{
    public class AssignmentViewModel
    {
        public int AssignmentId { get; set; }
        public string AssignmentName { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string PDFPath { get; set; }
        public string UploadAssignmentPath { get; set; }
        public DateTime AssignmentUploadDate { get; set; }
        public string AssignmentGrade { get; set; }
        public string AssigmentRemarks { get; set; }
        public bool IsUploaded { get; set; }
        public string SubjectName { get; set; }
    }
}
