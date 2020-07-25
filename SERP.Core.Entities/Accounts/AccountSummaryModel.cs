using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Entities.Accounts
{
    public class AccountSummaryModel
    {
        public string AccountName { get; set; }
        public int TotalTransactionCount { get; set; }
        public int TotalIncomeTransactionCount { get; set; }
        public int TotalExpenseTransactionCount { get; set; }
        public decimal   NetExpenseAmount { get; set; }
        public decimal   NetIncomeAmount { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
