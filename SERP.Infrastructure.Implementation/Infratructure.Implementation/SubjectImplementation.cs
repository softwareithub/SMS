using SERP.Core.Entities.Context;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class SubjectImplementation :GenericImplementation<SubjectMaster, int>, ISubjectMasterRepo
    {
        private SERPContext baseContext = null;
        public SubjectImplementation()
        {
            baseContext = new SERPContext();
        }
        public async Task<ResponseStatus> DeleteMultipleSubject(List<int> ids)
        {
            using (baseContext)
            {
                var models = baseContext.SubjectMasters.Where(x => ids.Contains(x.Id));
                List<SubjectMaster> updateModels = new List<SubjectMaster>();
                foreach(var data in models)
                {
                    data.IsActive = 0;
                    data.IsDeleted = 1;
                    data.UpdatedBy = 1;
                    data.UpdatedDate = DateTime.Now.Date;
                    updateModels.Add(data);
                }

                try
                {
                    baseContext.SubjectMasters.UpdateRange(updateModels);
                    await baseContext.SaveChangesAsync();
                    return ResponseStatus.UpdatedSuccessFully;
                }
                catch(Exception ex)
                {
                    return ResponseStatus.ServerError;
                }
            }
        }
    }
}
