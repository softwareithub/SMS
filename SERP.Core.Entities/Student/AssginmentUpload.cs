using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Student
{
    [Table("StudentAssignment",Schema = "Master")]
    public class AssginmentUpload:Base<int>
    {
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public string UploadAssignmentPath { get; set; }
        public DateTime AssignmentUploadDate { get; set; }
        public string AssignmentGrade { get; set; }
        public string AssigmentRemarks { get; set; }
    }
}
