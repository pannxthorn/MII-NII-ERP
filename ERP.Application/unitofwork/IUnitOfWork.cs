using ERP.Application.repositories;
using ERP.Domain.entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.unitofwork
{
    public interface IUnitOfWork : IDisposable
    {
        #region [Repository]
        IRepository<User> Users { get; }
        IRepository<Company> Company { get; }
        IRepository<Branch> Branch { get; }

        #endregion [Repository]

        Task SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task ExecuteInTransactionAsync(Func<Task> operation);
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);
    }
}
