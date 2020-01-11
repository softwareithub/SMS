using Microsoft.EntityFrameworkCore;
using SERP.Core.Entities.Context;
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
    public class GenericImplementation<TEntity, T> : IGenericRepository<TEntity, T> where TEntity : class
    {
        private  SERPContext baseContext = null;
        private DbSet<TEntity> TEntities = null;

        public GenericImplementation()
        {
            baseContext = new SERPContext();
            TEntities = baseContext.Set<TEntity>();
        }

        public  async Task<ResponseStatus> CreateEntity(TEntity model)
        {
            try
            {
                await TEntities.AddAsync(model);
                await baseContext.SaveChangesAsync();
                return ResponseStatus.AddedSuccessfully;
            }
            catch (Exception ex)
            {
                return ResponseStatus.ServerError;
            }
        }

        public  async Task<IList<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            List<TEntity> tList;
            using (baseContext)
            {
                IQueryable<TEntity> dbQuery = baseContext.Set<TEntity>();
                foreach (Expression<Func<TEntity, object>> navigationProp in navigationProperties)
                {
                    dbQuery.Include<TEntity, object>(navigationProp);
                }
                tList = await dbQuery.AsNoTracking().ToListAsync<TEntity>();
            }
            return tList;
        }

        public  async Task<IList<TEntity>> GetList(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            List<TEntity> list;
            using (baseContext)
            {
                IQueryable<TEntity> dbQuery = baseContext.Set<TEntity>();

                //Apply eager loading
                foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<TEntity, object>(navigationProperty);
                }

                list = dbQuery.AsNoTracking().Where(where).ToList<TEntity>();
            }
            return await Task.Run(() => list);
        }

        public  async Task<TEntity> GetSingle(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            TEntity item = null;
            using (baseContext)
            {
                IQueryable<TEntity> dbQuery = baseContext.Set<TEntity>();

                //Apply eager loading
                foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<TEntity, object>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return await Task.Run(() => item);
        }

        public  async Task<ResponseStatus> Add(params TEntity[] items)
        {
            try
            {
                using (baseContext)
                {
                    await baseContext.AddRangeAsync(items);
                    await baseContext.SaveChangesAsync();
                }

                return ResponseStatus.AddedSuccessfully;
            }
            catch (Exception ex)
            {
                return ResponseStatus.ServerError;
            }

        }

        public  async Task<ResponseStatus> Update(params TEntity[] items)
        {
            try
            {
                using (baseContext)
                {
                    baseContext.UpdateRange(items);
                    await baseContext.SaveChangesAsync();
                }
                return ResponseStatus.UpdatedSuccessFully;
            }
            catch (Exception ex)
            {
                return ResponseStatus.ServerError;
            }

        }

        public  async Task<ResponseStatus> Delete(params TEntity[] items)
        {
            try
            {
                using (baseContext)
                {
                    baseContext.UpdateRange(items);
                    await baseContext.SaveChangesAsync();
                }
                return ResponseStatus.DeletedSuccessfully;
            }
            catch (Exception ex)
            {
                return ResponseStatus.ServerError;
            }

        }

        public Task<IList<T>> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateNewContext()
        {
            baseContext = new SERPContext();
            return await Task.Run(() => true);
        }

       
    }
}
