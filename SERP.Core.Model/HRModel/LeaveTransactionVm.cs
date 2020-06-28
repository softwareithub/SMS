using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.HRModel
{
   public class LeaveTransactionVm
    {
        public int LeaveId { get; set; }
        public string LeaveName { get; set; }
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ReasonForLeave { get; set; }
        public string LeaveStatus { get; set; }
        public DateTime ApproveDate { get; set; }
        public DateTime RejectDate { get; set; }
        public string RejectReason { get; set; }
    }
}
