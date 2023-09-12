using AvironSofwateTest.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.EmployeeService.Dtos
{
    public class ReadEmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public int Rate { get; set; }
    }
}
