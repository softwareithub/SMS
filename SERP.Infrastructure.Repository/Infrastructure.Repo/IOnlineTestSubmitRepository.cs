using SERP.Core.Model.OnlineTest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface IOnlineTestSubmitRepository
    {
        Task<TestSubmissionModel> GetOnlineSubmitDetail(int userId, int testId);
    }
}
