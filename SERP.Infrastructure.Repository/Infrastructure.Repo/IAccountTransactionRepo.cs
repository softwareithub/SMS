using SERP.Core.Entities.Accounts;
using SERP.Core.Model.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface IAccountTransactionRepo
    {
        Task<List<AccountTransactionModel>> GetAccountTransactionDetails();
        Task<List<AccountSummaryModel>> GetAccountTransactionSummaryDetails();
    }
}
