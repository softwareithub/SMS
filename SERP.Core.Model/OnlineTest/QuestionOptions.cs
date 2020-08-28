using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.OnlineTest
{
    public class QuestionOptions
    {
        public string Question { get; set; }
        public string Options { get; set; }
        public int IsChecked { get; set; } = 0;
        public string DisplayOrder { get; set; }
        public Guid OptionTempId { get; set; }
        public string Action { get; set; }
    }
}
