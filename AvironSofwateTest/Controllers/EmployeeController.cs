using AvironSofwateTest.Core.DepartmentServices;
using AvironSofwateTest.Core.EmployeeService;
using AvironSofwateTest.Core.EmployeeService.Dtos;
using AvironSofwateTest.DataAccess.DataTable;
using Microsoft.AspNetCore.Mvc;

namespace AvironSofwateTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
                _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IDatatableResultModel<ReadEmployeeDto>> GetDepartment([FromQuery] DatatableQueryModel datatableQuery, CancellationToken cancellationToken)
        {
            return await _employeeService.GetAsync(datatableQuery, cancellationToken);
        }

        [HttpGet("{Id}")]
        public async Task<ReadEmployeeDto> GetDepartment(Guid Id, CancellationToken cancellationToken)
        {
            return await _employeeService.GetAsync(Id, cancellationToken);
        }

        [HttpPost]
        public async Task<int> CreateDepartment(CreateEmployeeDto model, CancellationToken cancellationToken)
        {
            return await _employeeService.CreateAsync(model, cancellationToken);
        }

        [HttpPut]
        public async Task<bool> UpdateDepartment(UpdateEmployeeDto model, CancellationToken cancellationToken)
        {
            return await _employeeService.UpdateAsync(model, cancellationToken);
        }

        [HttpDelete("{Id}")]
        public async Task<bool> DeleteDepartment(Guid Id, CancellationToken cancellationToken)
        {
            return await _employeeService.DeleteAsync(Id, cancellationToken);
        }
    }
}
