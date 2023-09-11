using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.DataAccess.Interface
{
    public interface IUnitOfWork<out TContext> : IDisposable where TContext : DbContext, new()
    {
        IRepository<TEntity> GetEntityRepository<TEntity>() where TEntity : class;
        Task<int> SaveAsync();
        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}
