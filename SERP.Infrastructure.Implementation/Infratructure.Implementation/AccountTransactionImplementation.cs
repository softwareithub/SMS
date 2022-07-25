using Helpers;
using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Accounts;
using SERP.Core.Entities.Context;
using SERP.Core.Model.Account;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.SqlHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class AccountTransactionImplementation : IAccountTransactionRepo
    {
        private SERPContext baseContext = null;
        private readonly string _connectionString;

        public AccountTransactionImplementation()
        {
            baseContext = new SERPContext();
            _connectionString = baseContext.Database.GetDbConnection().ConnectionString;
        }
        public async Task<List<AccountTransactionModel>> GetAccountTransactionDetails()
        {
            List<AccountTransactionModel> models = new List<AccountTransactionModel>();
            var commandText = "proc_GetAccountTransactionDetail";
            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, null);
                while (result.Read())
                {
                    AccountTransactionModel model = new AccountTransactionModel();
                    model.Id = result.DefaultIfNull<int>("Id");
                    model.AccountName = result.DefaultIfNull<string>("AccountName");
                    model.OpeningBalance = result.DefaultIfNull<decimal>("OpeningBalance");
                    model.IncomeName = result.DefaultIfNull<string>("IncomeName");
                    model.ExpenseName = result.DefaultIfNull<string>("ExpenseName");
                    model.Amount = result.DefaultIfNull<decimal>("Amount");
                    model.Purpose = result.DefaultIfNull<string>("Purpose");
                    model.RecieptNumber = result.DefaultIfNull<string>("RecieptNumber");
                    model.IncomeExpenseDate = result.DefaultIfNull<DateTime>("DateOfIncomeExpense");
                    models.Add(model);
                }

            return models;
        }

        public async Task<List<AccountSummaryModel>> GetAccountTransactionSummaryDetails()
        {
            List<AccountTransactionModel> models = new List<AccountTransactionModel>();
            List<AccountSummaryModel> modelSummaryList = new List<AccountSummaryModel>();
            var commandText = "proc_GetAccountTransactionDetail";
            var result = await SqlHelperExtension.ExecuteReader(_connectionString, commandText, System.Data.CommandType.StoredProcedure, null);
            while (result.Read())
            {
                AccountTransactionModel model = new AccountTransactionModel();
                model.Id = result.DefaultIfNull<int>("Id");
                model.AccountName = result.DefaultIfNull<string>("AccountName");
                model.OpeningBalance = result.DefaultIfNull<decimal>("OpeningBalance");
                model.IncomeName = result.DefaultIfNull<string>("IncomeName");
                model.ExpenseName = result.DefaultIfNull<string>("ExpenseName");
                model.Amount = result.DefaultIfNull<decimal>("Amount");
                model.Purpose = result.DefaultIfNull<string>("Purpose");
                model.RecieptNumber = result.DefaultIfNull<string>("RecieptNumber");
                model.IncomeExpenseDate = result.DefaultIfNull<DateTime>("DateOfIncomeExpense");
                models.Add(model);
            }

            foreach(var data in models.GroupBy(x=>x.AccountName))
            {
                int expenseCount = 0;
                int incomeCount = 0;
                decimal expenseAmount = default(decimal);
                decimal incomeAmount = default(decimal);

                AccountSummaryModel sumaryModel = new AccountSummaryModel();
                sumaryModel.AccountName = data.First().AccountName;
                sumaryModel.OpeningBalance = data.First().OpeningBalance;
                sumaryModel.TotalExpenseTransactionCount = data.Where(x => x.ExpenseName != null).Count();
                sumaryModel.TotalIncomeTransactionCount = data.Where(x => x.IncomeName != null).Count();
                foreach(var item in data)
                {
                    if (!string.IsNullOrEmpty(item.IncomeName))
                    {
                        incomeCount += 1;
                        incomeAmount += item.Amount;
                    }
                    else
                    {
                        expenseCount += 1;
                        expenseAmount += item.Amount;
                    }
                }
                sumaryModel.NetExpenseAmount = expenseAmount;
                sumaryModel.NetIncomeAmount = incomeAmount;

                modelSummaryList.Add(sumaryModel);
            }
            return modelSummaryList;
        }
    }
}
