using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("StudentFeeDeposit", Schema = "TransactionSch")]
    public class FeeDeposit :Base<int>
    {
        public int StudentId { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FineAmount { get; set; }
        public string ReasonFine { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime DateOfDeposit { get; set; }
        public int AcademicYear { get; set; }
    }
}
