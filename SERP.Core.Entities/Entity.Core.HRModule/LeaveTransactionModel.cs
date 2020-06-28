using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("LeaveTransaction")]
    public class LeaveTransactionModel:Base<int>
    {
        [Required(ErrorMessage ="Please select Leave Type")]
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "From Date is required")]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [Required(ErrorMessage = "To Date is required")]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }
        [Required(ErrorMessage ="Reason for Leave required.")]
        public string ReasonForLeave { get; set; }
        public string LeaveStatus { get; set; }
        public DateTime? ApproveDate { get; set; }
        public DateTime? RejectDate { get; set; }
        public string RejectReason { get; set; }
    }
}

