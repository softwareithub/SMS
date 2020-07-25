using SERP.Core.Model.FeeDetails;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface IFeeDepositRecieptRepo
    {
        Task<List<FeeRecieptModel>> GetStudentFeeReciept(int feeDepositId);
    }
}
