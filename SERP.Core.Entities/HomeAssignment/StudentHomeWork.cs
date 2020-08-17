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
        public int StudentId { get; set; } = default;
        public int HomeWorkId { get; set; } = default;
        [DataType(DataType.Date)]
        public DateTime SubmissionDate { get; set; } = default;
        public int IsSubmitted { get; set; } = default;
        public string Grade { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public int SessionId { get; set; }
    }
}
