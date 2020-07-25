using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.FeeDetails
{
    public class FeeRecieptModel
    {
        public int FeeDepositId { get; set; }
        public string RollCode { get; set; }
        public string Registration { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Address { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FineAmount { get; set; }
        public string ReasonForFine { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime DateOfDeposit { get; set; }
        public string CategoryName { get; set; }
        public string PaymentFor { get; set; }
        public decimal CategoryPayment { get; set; }
        public string FatherPhone { get; set; }

    }
}
