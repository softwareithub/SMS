using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.ExamDetail
{
    [Table("OptionMaster",Schema = "OnlineTest")]
    public class OptionMaster: Base<int>
    {
        public int QuestionId { get; set; } = default;
        public string OptionData { get; set; } = string.Empty;
        public int IsCorrectAnswere { get; set; } = default;
        public int SortOrder { get; set; }
        public int SessionId { get; set; }
    }
}
