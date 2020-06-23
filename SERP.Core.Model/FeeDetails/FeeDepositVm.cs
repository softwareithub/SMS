using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.FeeDetails
{
    public class FeeDepositVm
    {
        public int Id { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FineAmount { get; set; }
        public string FineReason { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime DepositDate { get; set; }
        public string CategoryName { get; set; }
        public string PaymentFor { get; set; }
        public string PaymentType { get; set; }
    }
}
