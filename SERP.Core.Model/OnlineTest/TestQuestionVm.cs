using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.OnlineTest
{
    public class TestQuestionVm
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public int TestQuestionCount { get; set; }
        public int TestTotalNumber { get; set; }
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public int QuestionMark { get; set; }
        public string TimeLimit { get; set; }
    }
}
