using AvironSofwateTest.Core.ReviewService.Dtos;
using AvironSofwateTest.DataAccess.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.ReviewService
{
    public interface IReviewService
    {
        Task<ReadReviewDto> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IDatatableResultModel<ReadReviewDto>> GetAsync(DatatableQueryModel queryModel, CancellationToken cancellationToken);
        Task<int> CreateAsync(CreateReviewDto model, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(UpdateReviewDto model, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
