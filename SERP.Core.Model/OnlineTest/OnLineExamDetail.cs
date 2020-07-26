using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.OnlineTest
{
    public class OnLineExamDetail
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int Sequence { get; set; }
        public string Question { get; set; }
        public List<Options> Options { get; set; }
        public int QuestionPoint { get; set; }
        public string AnswereType { get; set; }
    }

    public class Options
    {
        public int OptionId { get; set; }
        public string Option { get; set; }
        public bool  IsSelected { get; set; }
    }
}
