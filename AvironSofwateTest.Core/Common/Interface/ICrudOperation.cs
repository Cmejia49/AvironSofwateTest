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
        Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken);
        Task<int> CreateAsync(T model);
        Task<bool> UpdateAsync(Guid id, T model);
        Task<bool> DeleteAsync(Guid id);
    }
}
