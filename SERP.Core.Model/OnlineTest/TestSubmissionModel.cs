using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.OnlineTest
{
    public class TestSubmissionModel
    {
        public string TestName { get; set; }
        public int AttemptedQuestion { get; set; }
        public int UnAttemptedQuestion { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }
        public int TotalQuestion { get; set; }
    }
}
