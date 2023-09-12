using AutoMapper;
using AvironSofwateTest.Core.Common;
using AvironSofwateTest.Data.Entites;
using AvironSofwateTest.DataAccess.Context;
using AvironSofwateTest.DataAccess.DataTable;
using AvironSofwateTest.DataAccess.Interface;
using AvironSofwateTest.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.DepartmentServices
{
    public class DepartmentService : CommonService, IDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Department> _departmentRepository;

        public DepartmentService(IUnitOfWork<ApplicationDbContext> unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
            _departmentRepository  = unitOfWork.GetEntityRepository<Department>();
        }

        public async Task<int> CreateAsync(CreateDepartmentDto model, CancellationToken cancellationToken)
        {

            _departmentRepository.Create(_mapper.Map<CreateDepartmentDto, Department>(model));
            return await UnitOfWork.SaveAsync(cancellationToken) ;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _departmentRepository.DeleteAsync(cancellationToken, id);
            return await UnitOfWork.SaveAsync(cancellationToken) > 0 ;
        }

        public async Task<ReadDepartmentDto> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.Get().Where(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<Department, ReadDepartmentDto>(department);
        }

        public async Task<IDatatableResultModel<ReadDepartmentDto>> GetAsync(DatatableQueryModel queryModel, CancellationToken cancellationToken)
        {

                queryModel.SetDefaultSortingColumnIfEmpty(nameof(ReadDepartmentDto.Name), true);

                var departments = _departmentRepository.Get().Select(x => new ReadDepartmentDto { 
                 Id = x.Id,
                 Name = x.Name,
                }).ApplyDatatableQuery(queryModel.Filters);

                return new DatatableResultModel<ReadDepartmentDto>
                {
                    Data = await departments.ToOrderPageListAsync(queryModel,cancellationToken),
                    Total = await departments.CountAsync(cancellationToken)
                };
            
        }

        public async Task<bool> UpdateAsync(UpdateDepartmentDto model, CancellationToken cancellationToken)
        {
            await _departmentRepository.UpdateAsync(model,model.Id, cancellationToken);
            return await UnitOfWork.SaveAsync(cancellationToken) > 0;
        }
    }
}
