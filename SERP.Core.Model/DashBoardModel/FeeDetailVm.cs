using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace SERP.Core.Model.DashBoardModel
{
    public class FeeDetailVm
    {
        public decimal PayableAmountTillDate { get; set; }
        public decimal DiscountAmountTillDate { get; set; }
        public decimal FineAmountTillDate { get; set; }
        public decimal AmountPaidTillDate { get; set; }
        public decimal AmountDueTillDate { get; set; }
        public string MonthYearAcademic { get; set; }

    }
}
