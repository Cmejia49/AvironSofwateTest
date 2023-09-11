using AvironSofwateTest.DataAccess.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.DataAccess.Interface
{
    public interface IRepository
    {
    }
    public interface IRepository<TEntity> : IRepository where TEntity : class
    {
        IQueryable<TEntity> Get();

        IQueryable<TEntity> Get(Filters filters);

        IQueryable<TEntity> Get(Filters filters, SortingOptions sorting);

        Task<IDatatableResultModel<TEntity>> GetDatatableAsync(DatatableQueryModel queryModel, CancellationToken cancellationToken);

        Task<bool> AnyAsync(CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        TEntity Find(params object[] id);

        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] id);

        TEntity Create(TEntity entity);

        Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        Task<TEntity> UpdateAsync(object entityToUpdate, object[] id, CancellationToken cancellationToken);

        Task<TEntity> UpdateAsync(object entityToUpdate, object id, CancellationToken cancellationToken);

        Task<TEntity> DeleteAsync(CancellationToken cancellationToken, params object[] id);

        void Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);

        Task DeleteAllAsync();
    }
}
