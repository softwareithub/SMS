using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.TimeSheet
{
    public class TimeSheetVm
    {
        public string ClassName { get; set; }
        public string BatchName { get; set; }
        public string SubjectName { get; set; }
        public DateTime? TimeSheetDate { get; set; }
        public string Period { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
