using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CPC.Infraestructure.Crosscutting.DataObjects.Contracts
{
    public interface IRepository
    {
        Task<List<T>> GetTable<T>() where T : class;
        Task<List<T>> FromSql<T>(string sql, params object[] parameters) where T : class;
        Task<List<T>> Filter<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<List<T>> Filter<T>(Expression<Func<T, bool>> predicate, string navigations = "") where T : class;
        Task<List<T>> Filter<T>(Expression<Func<T, bool>> predicate, string[] navigations) where T : class;
        Task<T> GetByID<T>(params object[] id) where T : class;

        Task<bool> Delete<T>(T entity) where T : class;
        Task<T> Create<T>(T entity) where T : class;
        Task<bool> Update<T>(T entity) where T : class;
        Task<T> Update<T>(T oldEntity, T newEntity) where T : class;

        //Task<List<T>> ExecSqlQuery<T>(string qryOrProcedureName, CommandType commandType, DynamicParameters dbParameter = null);
        //Task<int> ExecSqlNonQuery(string qryOrProcedureName, CommandType commandType, DynamicParameters dbParameter = null);
    }
}
