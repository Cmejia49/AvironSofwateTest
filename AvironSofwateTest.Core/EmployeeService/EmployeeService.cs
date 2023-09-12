using AutoMapper;
using AvironSofwateTest.Core.Common;
using AvironSofwateTest.Core.DepartmentServices;
using AvironSofwateTest.Core.EmployeeService.Dtos;
using AvironSofwateTest.Core.ReviewService;
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

namespace AvironSofwateTest.Core.EmployeeService
{
    public class EmployeeService : CommonService, IEmployeeService
    {

        private readonly IMapper _mapper;

        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Review> _reviewRepository;


        public EmployeeService(IUnitOfWork<ApplicationDbContext> unitOfWork, IMapper mapper) : base(unitOfWork) 
        {
             _mapper = mapper;
            _employeeRepository = unitOfWork.GetEntityRepository<Employee>();
            _reviewRepository = unitOfWork.GetEntityRepository<Review>();
        }

        public async Task<int> CreateAsync(CreateEmployeeDto model, CancellationToken cancellationToken)
        {
            _employeeRepository.Create(_mapper.Map<CreateEmployeeDto, Employee>(model));
            return await UnitOfWork.SaveAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _employeeRepository.DeleteAsync(cancellationToken, id);
            return await UnitOfWork.SaveAsync(cancellationToken) > 0;
        }

        public async Task<ReadEmployeeDto> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var resultQuery = GetEmployeeQuery();
            var data = await resultQuery.FirstOrDefaultAsync();
            return data;
        }

        public async Task<IDatatableResultModel<ReadEmployeeDto>> GetAsync(DatatableQueryModel queryModel, CancellationToken cancellationToken)
        {
            queryModel.SetDefaultSortingColumnIfEmpty(nameof(ReadEmployeeDto.Name), true);

            var resultQuery = GetEmployeeQuery().ApplyDatatableQuery(queryModel.Filters);
            var data = await resultQuery.ToOrderPageListAsync(queryModel, cancellationToken);

            return new DatatableResultModel<ReadEmployeeDto>
            {
                Data =  data,
                Total = await resultQuery.CountAsync(cancellationToken)
            };
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeDto model, CancellationToken cancellationToken)
        {
            await _employeeRepository.UpdateAsync(model, model.Id, cancellationToken);
            return await UnitOfWork.SaveAsync(cancellationToken) > 0;
        }

        private IQueryable<ReadEmployeeDto> GetEmployeeQuery()
        {
            var employees = _employeeRepository.Get().Include(x => x.Department);

            IQueryable<ReadEmployeeDto> resultQuery = from employee in employees
                                                      join review in _reviewRepository.Get()
                                                      on employee.Id equals review.EmployeeId
                                                      select new ReadEmployeeDto()
                                                      {
                                                          Id = employee.Id,
                                                          Name = employee.Name,
                                                          DepartmentName = employee.Department.Name,
                                                          Rate = review.Rate
                                                      };
            return resultQuery;
        }
    }
}
