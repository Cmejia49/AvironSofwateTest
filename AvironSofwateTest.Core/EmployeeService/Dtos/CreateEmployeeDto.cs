using AvironSofwateTest.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.EmployeeService.Dtos
{
    public record CreateEmployeeDto
    {
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
