using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.HomeAssignment
{
    [Table("HomeWork",Schema= "Master")]
    public class HomeWorkModel: Base<int>
    {
        public string Name { get; set; }
        public int BatchId { get; set; }
        public int CourseId { get; set; }
        public int SubjectId { get; set; }
        public int EmployeeId { get; set; }
        [DataType(DataType.Date)]
        public DateTime HomeWorkDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime HomeWorkSubmissionDate { get; set; }
        public string HomeWork { get; set; }
        public string PDFPath { get; set; }

    }
}
