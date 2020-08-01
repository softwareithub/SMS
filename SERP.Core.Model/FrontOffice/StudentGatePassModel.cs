using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.FrontOffice
{
    public class StudentGatePassModel
    {
        public int Id { get; set; }
        public string GuardianName { get; set; }
        public string StudentName { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime LeavingTime { get; set; }
        public string ReasonOfLeaving { get; set; }
    }
}
