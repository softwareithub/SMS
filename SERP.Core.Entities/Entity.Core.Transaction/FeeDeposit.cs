using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("StudentFeeDeposit", Schema = "TransactionSch")]
    public class FeeDeposit :Base<int>
    {
        public int StudentId { get; set; } = default;
        public decimal PayableAmount { get; set; } = default;
        public decimal DiscountAmount { get; set; } = default;
        public decimal FineAmount { get; set; } = default;
        public string ReasonFine { get; set; } = default;
        public decimal AmountPaid { get; set; } = default;
        public decimal DueAmount { get; set; } = default;
        public DateTime DateOfDeposit { get; set; } = default;
        public int AcademicYear { get; set; } = default;
        public int SessionId { get; set; }
    }
}
