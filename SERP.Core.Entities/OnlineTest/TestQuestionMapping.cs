using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.OnlineTest
{
    [Table("TestQuestionMapping", Schema = "OnlineTest")]
    public class TestQuestionMapping: Base<int>
    {
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public int QuestionMark { get; set; }
    }
}
