using ERP.Application.repositories;
using ERP.Domain.entities;
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

        #endregion [Repository]

        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
