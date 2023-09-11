using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.DataAccess.DataTable
{
    public class DatatableGroupFilterParam
    {
        public string LogicalOperator { get; set; }
        public ICollection<DatatableFilterParam> Filters { get; set; }

        public DatatableGroupFilterParam Clone()
        {
            return CloneWithoutColumnFilter(null);
        }

        public DatatableGroupFilterParam CloneWithoutColumnFilter(string columnName)
        {
            return new DatatableGroupFilterParam
            {
                LogicalOperator = LogicalOperator,
                Filters = Filters.Where(t => columnName == null || t.ColumnName != columnName).Select(t => t.Clone()).ToList()
            };
        }
    }

    public class DatatableFilterParam
    {
        public string ColumnName { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public string LogicalOperator { get; set; }
        public string DatePart { get; set; }

        public DatatableFilterParam Clone()
        {
            return new DatatableFilterParam
            {
                ColumnName = ColumnName,
                Operator = Operator,
                Value = Value,
                DatePart = DatePart,
                LogicalOperator = LogicalOperator,
            };
        }
    }
}
