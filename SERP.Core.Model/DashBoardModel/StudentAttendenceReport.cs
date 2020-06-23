using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.DashBoardModel
{
    public class StudentAttendenceReport
    {
        public int   PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public string CourseBatchName { get; set; }
    }
}
