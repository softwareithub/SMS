using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("FeeDetail", Schema= "TransactionSch")]
    public class FeeDetailClassWise: Base<int>
    {
        public int CategoryId { get; set; } = default;
        public int ClassId { get; set; } = default;
        [Required(ErrorMessage = "Fee Amount is required")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; } = default;
        public string FeePaymentType { get; set; } = string.Empty;
        [NotMapped]
        public string CategoryName { get; set; }
        public int Type { get; set; } = default;
        public int AcademicYear { get; set; }
    }
}
