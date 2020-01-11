using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Utilities.ResponseUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Repository.Infrastructure.Repo
{
    public interface ISubjectMasterRepo :IGenericRepository<SubjectMaster, int>
    {
        Task<ResponseStatus> DeleteMultipleSubject(List<int> ids);
    }
}
