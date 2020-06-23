using SERP.Core.Model.FeeDetails;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface IFeeDetailRepo
    {
        Task<List<StudentFeeDetailVm>> GetFeeDetailRepo(int studentId);

        Task<List<FeeDepositVm>> GetPaymentHistory(int studentId);
    }
}
