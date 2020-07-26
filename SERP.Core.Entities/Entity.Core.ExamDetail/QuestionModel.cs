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
        public string Question { get; set; } = string.Empty;
        public int QuestionPoint { get; set; } = default;
        public int SubjectId { get; set; } = default;
        public int CourseId { get; set; } = default;
        public string AnswereType { get; set; } = "SingleSelect";
    }
}
