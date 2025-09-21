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
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<TResult>> GetManyAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);

        // Command Methods

        // Count Methods
    }
}
