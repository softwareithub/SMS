using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("FeeDiscountParticularWise",Schema= "TransactionSch")]
    public class FeeDiscountParticularWiseModel:Base<int>
    {
        public int FeeDiscountId { get; set; }
        public int ParticularId { get; set; }
        public string DiscountType { get; set; }
        public string Description { get; set; }
        public decimal DiscountValue { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }

    }
}
