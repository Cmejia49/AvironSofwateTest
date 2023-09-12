using AvironSofwateTest.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.DepartmentServices
{
    public class UpdateDepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
