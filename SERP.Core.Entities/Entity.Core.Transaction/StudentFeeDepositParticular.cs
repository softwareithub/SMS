using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("FeeDepositParticular",Schema = "TransactionSch")]
    public class StudentFeeDepositParticular :Base<int>
    {
        public int StudentFeeDepositId { get; set; }
        public int ParticularId { get; set; }
        public string PaymentFor { get; set; }
        public decimal Amount { get; set; }
    }
}
