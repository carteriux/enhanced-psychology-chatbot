using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CPC.Infraestructure.Crosscutting.DataObjects.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CPC.Infraestructure.Crosscutting.DataObjects.Core
{
    public class GenericRepository<TContext, T> : IGenericRepository<T> where T : class, new()
        where TContext : DbContext
    {
        protected DbContext dbContext;
        public readonly IUnitOfWork unitOfWork;
        private DbSet<T> dbSet;

        public GenericRepository(TContext context)
        {
            this.dbContext = context;
            this.unitOfWork = new UnitOfWork(context);
            this.dbSet = this.dbContext.Set<T>();
        }

        public async Task<T> Create(T entity)
        {
            var result = entity;
            try
            {
                var eEntry = await this.dbSet.AddAsync(entity);
                result = eEntry.Entity;
                await this.unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                var e = ex.Message;
                this.unitOfWork.Dispose();
            }
            return result;
        }

        public async Task Delete(T entity)
        {
            try
            {
                var dbSet = this.dbSet;
                if (this.dbContext.Entry(entity).State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }
                dbSet.Remove(entity);
                await this.unitOfWork.Commit();
            }
            catch
            {
                this.unitOfWork.Dispose();
            }
        }

        public async Task<List<T>> Filter(Expression<Func<T, bool>> predicate)
        {
            return await this.dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> GetByID(params object[] id)
        {
            return await this.dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetTable()
        {
            return await this.dbSet.ToListAsync();
        }

        public async Task<T> Update(T entity)
        {
            try
            {
                var dbSet = this.dbSet;
                if (this.dbContext.Entry(entity).State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }
                this.dbContext.Entry(entity).State = EntityState.Modified;
                await this.unitOfWork.Commit();
            }
            catch
            {
                this.unitOfWork.Dispose();
            }
            return entity;
        }

    }
}
