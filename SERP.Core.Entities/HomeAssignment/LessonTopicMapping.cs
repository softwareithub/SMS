using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.HomeAssignment
{
    [Table("LessonTopicMapping", Schema ="Master")]
    public class LessonTopicMapping :Base<int>
    {
        public int LessonId { get; set; } = default;
        public string TopicName { get; set; } = default;

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; } = default;
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; } = default;
        public string TopicDescription { get; set; } = string.Empty;
        public int ExpectedClass { get; set; } = default;
    }
}
