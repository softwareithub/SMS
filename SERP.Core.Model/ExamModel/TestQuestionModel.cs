using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.OnlineTest;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.ExamModel
{
    public class TestQuestionModel
    {
        public int QuestionId { get; set; }
        public int TestId { get; set; }
        public string SubjectName { get; set; }
        public string CourseName { get; set; }
        public string Question { get; set; }
        public int QuestionPoint { get; set; }

    }
}
