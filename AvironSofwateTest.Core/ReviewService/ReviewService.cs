using AutoMapper;
using AvironSofwateTest.Core.Common;
using AvironSofwateTest.Core.DepartmentServices;
using AvironSofwateTest.Core.EmployeeService.Dtos;
using AvironSofwateTest.Core.ReviewService;
using AvironSofwateTest.Core.ReviewService.Dtos;
using AvironSofwateTest.Data.Entites;
using AvironSofwateTest.DataAccess.Context;
using AvironSofwateTest.DataAccess.DataTable;
using AvironSofwateTest.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.ReviewServoce
{
    public class ReviewService : CommonService, IReviewService
    {
        private readonly IMapper _mapper;

        private readonly IRepository<Review> _reviewRepository;

        public ReviewService(IUnitOfWork<ApplicationDbContext> unitOfWork , IMapper mapper) : base (unitOfWork)
        {
            _mapper = mapper;
            _reviewRepository = unitOfWork.GetEntityRepository<Review>();
        }

        public async Task<int> CreateAsync(CreateReviewDto model, CancellationToken cancellationToken)
        {
            _reviewRepository.Create(_mapper.Map<CreateReviewDto, Review>(model));
            return await UnitOfWork.SaveAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _reviewRepository.DeleteAsync(cancellationToken, id);
            return await UnitOfWork.SaveAsync(cancellationToken) > 0;
        }

        public async Task<ReadReviewDto> GetAsync(Guid id, CancellationToken cancellationToken)
        {

            var review = await _reviewRepository.Get()
                 .Include(x => x.Employee)
                 .ThenInclude(x => x.Department)
                 .Where(x => x.Id == id)
                 .Select(x => new ReadReviewDto
                 {
                     Rate = x.Rate,
                     EmployeeName = x.Employee.Name,
                     DepartmentName = x.Employee.Department.Name,
                 }).FirstOrDefaultAsync(cancellationToken);

            return review;
        }

        public async Task<IDatatableResultModel<ReadReviewDto>> GetAsync(DatatableQueryModel queryModel, CancellationToken cancellationToken)
        {
            queryModel.SetDefaultSortingColumnIfEmpty(nameof(ReadReviewDto.Rate), true);

            var review =  _reviewRepository.Get()
                 .Include(x => x.Employee)
                 .ThenInclude(x => x.Department).Select(x => new ReadReviewDto
                 {
                     Rate = x.Rate,
                     EmployeeName = x.Employee.Name,
                     DepartmentName = x.Employee.Department.Name,
                 }).ApplyDatatableQuery(queryModel.Filters);

            return new DatatableResultModel<ReadReviewDto>
            {
                Data = await review.ToOrderPageListAsync(queryModel, cancellationToken),
                Total = await review.CountAsync(cancellationToken)
            };

        }

        public async Task<bool> UpdateAsync(UpdateReviewDto model, CancellationToken cancellationToken)
        {
            await _reviewRepository.UpdateAsync(model, model.Id, cancellationToken);
            return await UnitOfWork.SaveAsync(cancellationToken) > 0;
        }
    }
}
