using AvironSofwateTest.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Data.Entites
{
    public class Employee : BaseAuditableEntity
    {
        public string Name { get; set; }
        public Review Review { get; set; }
    }
}
