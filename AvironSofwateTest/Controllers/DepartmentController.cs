using AvironSofwateTest.Core.DepartmentServices;
using AvironSofwateTest.DataAccess.DataTable;
using Microsoft.AspNetCore.Mvc;

namespace AvironSofwateTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IDatatableResultModel<ReadDepartmentDto>> GetDepartment([FromQuery] DatatableQueryModel datatableQuery, CancellationToken cancellationToken)
        {
            return await _departmentService.GetAsync(datatableQuery, cancellationToken);
        }

        [HttpGet("{Id}")]
        public async Task<ReadDepartmentDto> GetDepartment(Guid Id, CancellationToken cancellationToken)
        {
            return await _departmentService.GetAsync(Id, cancellationToken);
        }

        [HttpPost]
        public async Task<int> CreateDepartment(CreateDepartmentDto model, CancellationToken cancellationToken)
        {
            return await _departmentService.CreateAsync(model, cancellationToken);
        }

        [HttpPut]
        public async Task<bool> UpdateDepartment(UpdateDepartmentDto model, CancellationToken cancellationToken)
        {
            return await _departmentService.UpdateAsync(model, cancellationToken);
        }

        [HttpDelete("{Id}")]
        public async Task<bool> DeleteDepartment(Guid Id, CancellationToken cancellationToken)
        {
            return await _departmentService.DeleteAsync(Id, cancellationToken);
        }
    }
}
