using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CPC.Infraestructure.Crosscutting.DataObjects.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CPC.Infraestructure.Crosscutting.DataObjects.Core
{
    public class Repository<TContext> : IRepository where TContext : DbContext
    {
        public TContext dbContext;

        private readonly string connectionString;

        public readonly IUnitOfWork unitOfWork;

        public Repository(TContext context)
        {
            this.dbContext = context;
            this.unitOfWork = new UnitOfWork(context);
            this.connectionString = context.Database.GetConnectionString();
        }

        public async virtual Task<List<T>> GetTable<T>() where T : class
        {
            return await this.dbContext.Set<T>().ToListAsync();
        }

        public async virtual Task<List<T>> Filter<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await this.dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async virtual Task<List<T>> Filter<T>(Expression<Func<T, bool>> predicate, string navigations = "") where T : class
        {
            if (!string.IsNullOrEmpty(navigations))
            {
                return await this.dbContext.Set<T>().Where(predicate).Include(navigations).ToListAsync();
            }
            else
            {
                return await this.dbContext.Set<T>().Where(predicate).ToListAsync();
            }
        }
     
        public async virtual Task<List<T>> Filter<T>(Expression<Func<T, bool>> predicate, string [] navigations ) where T : class
        {
            IQueryable<T> query = null;
            if (navigations.Length>0)
            {

                query=  this.dbContext.Set<T>().Where(predicate);
                for (int i = 0; i < navigations.Length; i++)
                {
                    query = query.Include(navigations[i]);

                    
                }

                return await query.ToListAsync();
            }
            else
            {
                return await this.dbContext.Set<T>().Where(predicate).ToListAsync();
            }
        }

        public async virtual Task<T> GetByID<T>(params object[] id) where T : class
        {
            return await this.dbContext.Set<T>().FindAsync(id);
        }

        public async virtual Task<bool> Delete<T>(T entity) where T : class
        {
            try
            {
                var dbSet = this.dbContext.Set<T>();
                if (this.dbContext.Entry(entity).State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }
                dbSet.Remove(entity);
                await this.unitOfWork.Commit();
                return true;
            }
            catch
            {
                //this.unitOfWork.Dispose();
                return false;
            }
        }

        public async virtual Task<T> Create<T>(T entity) where T : class
        {
            var result = entity;
            try
            {
                var eEntry = await this.dbContext.Set<T>().AddAsync(entity);
                result = eEntry.Entity;
                await this.unitOfWork.Commit();
            }
            catch(Exception e)
            {
                //this.unitOfWork.Dispose();
            }
            return result;
        }

        public async virtual Task<bool> Update<T>(T entity) where T : class
        {
            var response = false;

            try
            {
                var dbSet = this.dbContext.Set<T>();
                var keyNames = this.dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name).ToList();
                var keyValues = keyNames.Select(keyName => typeof(T).GetProperty(keyName).GetValue(entity)).ToArray();
                var existingEntity = await dbSet.FindAsync(keyValues);
                if (existingEntity != null)
                {
                    this.dbContext.Entry(existingEntity).State = EntityState.Detached;
                }
                dbSet.Update(entity);
                return response = await this.unitOfWork.Commit() > 0;
            }
            catch (Exception ex)
            {
                //this.unitOfWork.Dispose();
            }

            return response;
        }

        public async virtual Task<T> Update<T>(T oldEntity, T newEntity) where T : class
        {
            try
            {
                this.dbContext.Entry(oldEntity).CurrentValues.SetValues(newEntity);
                await this.unitOfWork.Commit();
            }
            catch(Exception ex)
            {
                //this.unitOfWork.Dispose();
            }
            return newEntity;
        }

        //public async virtual Task<List<T>> ExecSqlQuery<T>(string qryOrProcedureName, CommandType commandType, DynamicParameters dbParameter = null)
        //{
        //    using IDbConnection db = new SqlConnection(this.connectionString);
        //    var result = await db.QueryAsync<T>(qryOrProcedureName, dbParameter, commandType: commandType);
        //    return result.AsList();
        //}

        //public async virtual Task<int> ExecSqlNonQuery(string qryOrProcedureName, CommandType commandType, DynamicParameters dbParameter = null)
        //{
        //    using IDbConnection db = new SqlConnection(this.connectionString);
        //    return await db.ExecuteAsync(qryOrProcedureName, dbParameter, commandType: commandType);
        //}

        public async Task<List<T>> FromSql<T>(string sql, params object[] parameters) where T : class
        {
            return await this.dbContext.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }

        public async Task<List<T>> SqlQuery<T>(string sql, params object[] parameters) where T : class
        {
            return await this.dbContext.Database.SqlQueryRaw<T>(sql, parameters).ToListAsync();
        }
    }
}
