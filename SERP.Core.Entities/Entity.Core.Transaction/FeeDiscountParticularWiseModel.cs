using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("FeeDiscountParticularWise",Schema= "TransactionSch")]
    public class FeeDiscountParticularWiseModel:Base<int>
    {
        public int FeeDiscountId { get; set; } = default;
        public int ParticularId { get; set; } = default;
        public string DiscountType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; } = default;
        [NotMapped]
        public string CategoryName { get; set; }
        public int SessionId { get; set; }
    }
}
