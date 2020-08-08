using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.QuickSearchModel
{
    public class StudentFeeDetailModel
    {
        public List<StudentPayment> PaymentModels { get; set; }
        public List<StudentFeeDpositDetail> StudentFeeDpositDetails { get; set; }

        public StudentFeeDisocunt FeeDiscountValue { get; set; }
    }

    public class StudentFeeDpositDetail
    {
        public int DepositId { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FineAmount { get; set; }
        public string Reason { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime DepositDate { get; set; }
    }

    public class StudentFeeDisocunt
    {
        public string DiscountName { get; set; }
        public string DiscountCode { get; set; }
        public string Description { get; set; }
        public string DiscountType { get; set; }
        public decimal DisocuntValue { get; set; }
    }

    public class StudentPayment
    {
        public string FeeParticular { get; set; }
        public decimal Amount { get; set; }
        public string FeePaymentType { get; set; }
        public string PaymentType { get; set; }
        public decimal YearlyPayment { get; set; }
    }
}
