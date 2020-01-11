using SERP.Core.Entities.Entity.Core.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.TransactionViewModel
{
    public class FeeDiscountViewModel
    {
        public List<FeeDiscountParticularWiseModel> FeeDiscountParticualVms { get; set; }
        public FeeDiscountModel FeeDiscountVm { get; set; }
    }
}
