using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("FeeDepositParticular",Schema = "TransactionSch")]
    public class StudentFeeDepositParticular :Base<int>
    {
        public int StudentFeeDepositId { get; set; } = default;
        public int ParticularId { get; set; } = default;
        public string PaymentFor { get; set; } = string.Empty;
        public decimal Amount { get; set; } = default;
    }
}
