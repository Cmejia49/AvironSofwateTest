using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Data.Common
{
    public class BaseAuditableEntity : BaseEntity
    {
        public DateTime Created { get; set; }
        public int CreatedById { get; set; }
        public string Operation { get; set; }
    }
}
