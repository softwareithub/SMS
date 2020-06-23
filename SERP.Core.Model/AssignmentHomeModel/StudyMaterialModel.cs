using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.AssignmentHomeModel
{
    public class StudyMaterialModel
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string Description { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string SubjectName { get; set; }
        public string UploadType { get; set; }
        public string Path { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
