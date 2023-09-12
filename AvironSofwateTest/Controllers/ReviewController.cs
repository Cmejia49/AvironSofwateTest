using AvironSofwateTest.Core.DepartmentServices;
using AvironSofwateTest.Core.EmployeeService;
using AvironSofwateTest.Core.EmployeeService.Dtos;
using AvironSofwateTest.Core.ReviewService;
using AvironSofwateTest.Core.ReviewService.Dtos;
using AvironSofwateTest.DataAccess.DataTable;
using Microsoft.AspNetCore.Mvc;

namespace AvironSofwateTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
                _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IDatatableResultModel<ReadReviewDto>> GetDepartment([FromQuery] DatatableQueryModel datatableQuery, CancellationToken cancellationToken)
        {
            return await _reviewService.GetAsync(datatableQuery, cancellationToken);
        }

        [HttpGet("{Id}")]
        public async Task<ReadReviewDto> GetDepartment(Guid Id, CancellationToken cancellationToken)
        {
            return await _reviewService.GetAsync(Id, cancellationToken);
        }

        [HttpPost]
        public async Task<int> CreateDepartment(CreateReviewDto model, CancellationToken cancellationToken)
        {
            return await _reviewService.CreateAsync(model, cancellationToken);
        }

        [HttpPut]
        public async Task<bool> UpdateDepartment(UpdateReviewDto model, CancellationToken cancellationToken)
        {
            return await _reviewService.UpdateAsync(model, cancellationToken);
        }

        [HttpDelete("{Id}")]
        public async Task<bool> DeleteDepartment(Guid Id, CancellationToken cancellationToken)
        {
            return await _reviewService.DeleteAsync(Id, cancellationToken);
        }
    }
}
