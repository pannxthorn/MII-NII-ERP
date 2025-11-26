using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.repositories
{
    public interface IRepository<T> where T : class
    {
        // Query Methods
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(int id);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, bool asNoTracking = true, bool useSplitQuery = true);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null, bool asNoTracking = true, bool useSplitQuery = true);
        Task<IEnumerable<TResult>> GetManyAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        IQueryable<T> GetQueryable(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool asNoTracking = true);

        // Command Methods
        Task AddAsync(T entity);

        // Count Methods
    }
}
