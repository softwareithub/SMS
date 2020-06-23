using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.HomeAssignment
{
    [Table("LessonTopicMapping", Schema ="Master")]
    public class LessonTopicMapping :Base<int>
    {
        public int LessonId { get; set; }
        public string TopicName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TopicDescription { get; set; }
        public int ExpectedClass { get; set; }
    }
}
