using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.OnlineVideo
{
    [Table("OnlineVideoFeeDetail")]
    public class OnlineVideoFeeDetail: Base<int>
    {
        public int CourseId { get; set; }
        public int SubjectId { get; set; }
        public decimal FeeAmount { get; set; }
        public string CourseDescription { get; set; }
    }
}
