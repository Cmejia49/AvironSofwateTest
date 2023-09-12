using AvironSofwateTest.Core.Common.Interface;
using AvironSofwateTest.DataAccess.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.DepartmentServices
{
    public interface IDepartmentService 
    {
        Task<ReadDepartmentDto> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IDatatableResultModel<ReadDepartmentDto>> GetAsync(DatatableQueryModel queryModel, CancellationToken cancellationToken);
        Task<int> CreateAsync(CreateDepartmentDto model, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(UpdateDepartmentDto model, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
