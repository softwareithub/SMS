using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.QuickSearchModel
{
    public class BookDetailModel
    {
        public int BookCount { get; set; }
        public string TitleName { get; set; }
        public string AuthorName { get; set; }
        public string Publisher { get; set; }
        public string StatusName { get; set; }
        public decimal CostPrice { get; set; }
        public string CourseName{ get; set; }
        public string SubjectName { get; set; }
        public string ColorCode { get; set; }
    }
}
