using AvironSofwateTest.DataAccess.Interface;
using AvironSofwateTest.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.DataAccess.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        private readonly Dictionary<Type, object> _repositoryDictionary = new();

        protected TContext _context;
        protected bool disposed;

        public UnitOfWork(TContext context)
        {
            _context = context;

            if (context == null)
            {
                throw new InvalidOperationException("Entity.DbContext instance is expected as a dbContext parameter.");
            }
        }

        public IRepository<TEntity> GetEntityRepository<TEntity>() where TEntity : class
        {
            if (_repositoryDictionary.TryGetValue(typeof(TEntity), out var value))
            {
                return value as IRepository<TEntity>;
            }
            var repo = new Repository<TEntity>(_context);
            _repositoryDictionary.Add(typeof(TEntity), repo);
            return repo;
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing && _context is IDisposable context)
            {
                context.Dispose();
            }
            disposed = true;
        }

    }
}
