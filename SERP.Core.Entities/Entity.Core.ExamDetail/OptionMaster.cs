using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.ExamDetail
{
    [Table("OptionMaster",Schema = "OnlineTest")]
    public class OptionMaster: Base<int>
    {
        public int QuestionId { get; set; }
        public string OptionData { get; set; }
        public int IsCorrectAnswere { get; set; }
    }
}
