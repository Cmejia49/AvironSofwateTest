using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.DataAccess.DataTable
{
    public interface IDatatableResultModel<out T>
    {
        int Total { get; }
        IEnumerable<T> Data { get; }
    }
}
