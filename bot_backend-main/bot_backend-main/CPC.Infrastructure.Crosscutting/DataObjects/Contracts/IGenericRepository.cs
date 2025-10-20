using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CPC.Infraestructure.Crosscutting.DataObjects.Contracts
{
    public interface IGenericRepository<T> where T : new()
    {
        Task<List<T>> GetTable();
        Task<T> GetByID(params object[] id);
        Task<List<T>> Filter(Expression<Func<T, bool>> predicate);
        Task Delete(T entity);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
    }
}
