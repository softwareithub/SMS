using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.Account
{
    public class AccountTransactionModel
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public decimal OpeningBalance { get; set; }
        public string IncomeName { get; set; }
        public string ExpenseName { get; set; }
        public decimal Amount { get; set; }
        public string RecieptNumber { get; set; }
        public string Purpose { get; set; }
        public DateTime IncomeExpenseDate { get; set; }
    }
}
