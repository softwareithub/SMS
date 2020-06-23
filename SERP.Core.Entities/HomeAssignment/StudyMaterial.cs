using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.HomeAssignment
{
    [Table("StudyMaterial", Schema = "Master")]
    public class StudyMaterial:Base<int>
    {
        public string MaterialName { get; set; }
        public string MaterialDescription { get; set; }
        public int CourseId { get; set; }
        public int BatchId { get; set; }
        public int SubjectId { get; set; }
        public string MaterialPath { get; set; }
        public DateTime PublishDate { get; set; }
        public string UploadType { get; set; }

    }
}
