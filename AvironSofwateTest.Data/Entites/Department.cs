using AvironSofwateTest.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Data.Entites
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<Employee> Employees { get; set; }
    }
}
