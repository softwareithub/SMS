using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.HomeAssignment
{
    [Table("StudentHomeWorkSubmission", Schema ="Master")]
    public class StudentHomeWork: Base<int>
    {
        public int StudentId { get; set; }
        public int HomeWorkId { get; set; }
        [DataType(DataType.Date)]
        public DateTime SubmissionDate { get; set; }
        public int IsSubmitted { get; set; }
        public string Grade { get; set; }
        public string Reason { get; set; }
      
    }
}
