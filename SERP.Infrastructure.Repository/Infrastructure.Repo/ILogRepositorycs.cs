using SERP.Core.Entities.TransactionLog;
using SERP.Utilities.ResponseUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface ILogRepositorycs
    {
        Task<ResponseStatus> CreateEntity(LogMaster model);
    }
}
