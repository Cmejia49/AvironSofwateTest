using AvironSofwateTest.Core.Common;
using AvironSofwateTest.Data.Entites;
using AvironSofwateTest.DataAccess.Context;
using AvironSofwateTest.DataAccess.Interface;
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
        private readonly IRepository<Department> _departmentRepository;

        public DepartmentService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
        {
            _departmentRepository  = unitOfWork.GetEntityRepository<Department>();
        }
        public Task<int> CreateAsync(DepartmentDto model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DepartmentDto> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //return await _departmentRepository.Get().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<IEnumerable<DepartmentDto>> GetAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Guid id, DepartmentDto model)
        {
            throw new NotImplementedException();
        }
    }
}
