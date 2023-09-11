using AvironSofwateTest.DataAccess.DataTable;
using AvironSofwateTest.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.DataAccess.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext _context;
        protected DbSet<TEntity> _set;

        public Repository(DbContext context)
        {
            _context = context;
            _set = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> Get()
        {
            return _set;
        }

        public IQueryable<TEntity> Get(Filters filters)
        {
            return _set.ApplyDatatableQuery(filters);
        }

        public IQueryable<TEntity> Get(Filters filters, SortingOptions sorting)
        {
            return _set.ApplyDatatableQuery(filters).OrderBySortingOptions(sorting);
        }

        public async Task<IDatatableResultModel<TEntity>> GetDatatableAsync(DatatableQueryModel queryModel, CancellationToken cancellationToken)
        {
            var query = _set.ApplyDatatableQuery(queryModel.Filters);
            return new DatatableResultModel<TEntity>
            {
                Data = await query.ToOrderPageListAsync(queryModel, cancellationToken),
                Total = await query.CountAsync(cancellationToken)
            };
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return _set.AnyAsync(cancellationToken);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return _set.AnyAsync(predicate, cancellationToken);
        }

        public TEntity Find(params object[] id)
        {
            return _set.Find(id);
        }

        public Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] id)
        {
            return _set.FindAsync(id, cancellationToken).AsTask();
        }

        public TEntity Create(TEntity entity)
        {
            var result = _set.Add(entity);
            return result.Entity;
        }

        public Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            return _set.AddRangeAsync(entities, cancellationToken);
        }

        public Task<TEntity> UpdateAsync(object entityToUpdate, object id, CancellationToken cancellationToken)
        {
            return UpdateAsync(entityToUpdate, new[] { id }, cancellationToken);
        }

        public async Task<TEntity> UpdateAsync(object entityToUpdate, object[] id, CancellationToken cancellationToken)
        {
            TEntity targetState = await FindAsync(cancellationToken, id);

            if (targetState == null)
                throw new KeyNotFoundException($"There are no records with id: {id}");

            _context.Entry(targetState).CurrentValues.SetValues(entityToUpdate);

            return targetState;
        }

        public async Task<TEntity> DeleteAsync(CancellationToken cancellationToken, params object[] id)
        {
            TEntity entityToDelete = await FindAsync(cancellationToken, id);

            if (entityToDelete != null)
                Delete(entityToDelete);

            return entityToDelete;
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (_context.Entry(entity).State == EntityState.Detached)
                _set.Attach(entity);

            _set.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                Delete(entity);
        }

        public async Task DeleteAllAsync()
        {
            await _context.Database.ExecuteSqlRawAsync($"DELETE FROM [{typeof(TEntity).Name}]");
        }
    }
}
