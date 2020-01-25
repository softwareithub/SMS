using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.TransactionViewModel
{
    public class ClassFeeAllocationVm
    {
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public string ClassName { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public int Type { get; set; }

    }
}
