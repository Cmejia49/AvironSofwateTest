using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.DataAccess.DataTable
{
    public class DatatableResultModel<T> : IDatatableResultModel<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
