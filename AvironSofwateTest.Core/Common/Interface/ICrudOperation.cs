using AvironSofwateTest.DataAccess.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.Common.Interface
{
    public interface ICrudOperation<T> where T : class
    {
        Task<T> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IDatatableResultModel<T>> GetAsync(DatatableQueryModel queryModel, CancellationToken cancellationToken);
        Task<int> CreateAsync(T model, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(T model, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
