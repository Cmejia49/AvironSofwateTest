using AvironSofwateTest.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Data.Entites
{
    public class Review : BaseAuditableEntity
    {
        public int Rate { get; set; }
    }
}
