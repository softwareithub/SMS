using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.ExamDetail
{
    [Table("QuestionMaster", Schema = "OnlineTest")]
    public class QuestionModel:Base<int>
    {
        public string Question { get; set; }
        public int QuestionPoint { get; set; }
        public int SubjectId { get; set; }
        public int CourseId { get; set; }
    }
}
