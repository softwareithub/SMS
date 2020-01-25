using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Context;
using SERP.Core.Entities.TransactionLog;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Implementation.Infratructure.Implementation
{
    public class LogImplementation : ILogRepositorycs
    {
        public async Task<ResponseStatus> CreateEntity(LogMaster model)
        {
        
            using (var context = new SERPContext())
            {
                DbSet<LogMaster> logMasters = context.Set<LogMaster>();
                await logMasters.AddAsync(model);
                await context.SaveChangesAsync();

                return ResponseStatus.AddedSuccessfully;

            }
        }
    }
}
