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
        public string MaterialName { get; set; } = string.Empty;
        public string MaterialDescription { get; set; } = string.Empty;
        public int CourseId { get; set; } = default;
        public int BatchId { get; set; } = default;
        public int SubjectId { get; set; } = default;
        public string MaterialPath { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; } = default;
        public string UploadType { get; set; } = string.Empty;

    }
}
