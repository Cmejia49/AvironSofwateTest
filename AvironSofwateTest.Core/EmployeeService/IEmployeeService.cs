using AvironSofwateTest.Core.EmployeeService.Dtos;
using AvironSofwateTest.DataAccess.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.EmployeeService
{
    public interface IEmployeeService
    {
        Task<ReadEmployeeDto> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IDatatableResultModel<ReadEmployeeDto>> GetAsync(DatatableQueryModel queryModel, CancellationToken cancellationToken);
        Task<int> CreateAsync(CreateEmployeeDto model, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(UpdateEmployeeDto model, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
