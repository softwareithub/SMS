using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.HomeAssignment
{
    [Table("StudyMaterial", Schema = "Master")]
    public class StudyMaterial:Base<int>
    {
        [Required(ErrorMessage ="Please Enter Title")]
        public string MaterialName { get; set; } = string.Empty;
        public string MaterialDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Select Course/Class")]
        public int CourseId { get; set; } = default;

        [Required(ErrorMessage = "Please Select Batch/Section")]
        public int BatchId { get; set; } = default;

        [Required(ErrorMessage = "Please Select Subject")]
        public int SubjectId { get; set; } = default;

        [Required(ErrorMessage = "Please Upload File")]
        public string MaterialPath { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; } = default;

        [Required(ErrorMessage = "Please Select Upload Type")]
        public string UploadType { get; set; } = string.Empty;

    }
}
