using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.DataAccess.DataTable
{
    public class DatatableSortingParam
    {
        public string ColumnName { get; set; }
        public bool Descending { get; set; }

        public DatatableSortingParam Clone()
        {
            return new DatatableSortingParam
            {
                ColumnName = ColumnName,
                Descending = Descending
            };
        }
    }
}
