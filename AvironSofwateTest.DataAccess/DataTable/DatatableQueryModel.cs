using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.DataAccess.DataTable
{
    public class DatatableQueryModel
    {
        public DatatableQueryModel()
        {
            Filters = new Filters();
            Sorting = new SortingOptions();
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public Filters Filters { get; set; }
        public SortingOptions Sorting { get; set; }

        public DatatableQueryModel Clone()
        {
            return CloneWithoutColumnFilter(null);
        }

        public DatatableQueryModel CloneWithoutColumnFilter(string columnName)
        {
            return new DatatableQueryModel
            {
                Filters = Filters.CloneWithoutColumnFilter(columnName),
                Sorting = Sorting.Clone(),
                Page = Page,
                PageSize = PageSize,
            };
        }

        public void SetDefaultSortingColumnIfEmpty(string columnName, bool isDescending = false)
        {
            if (Sorting == null)
            {
                Sorting = new SortingOptions();
            }
            if (!Sorting.Any())
            {
                Sorting.Add(new() { ColumnName = columnName, Descending = isDescending });
            }
        }

        public void SetDefaultSortingParamsIfEmpty(IEnumerable<DatatableSortingParam> datatableSortingParams)
        {
            if (Sorting == null)
            {
                Sorting = new SortingOptions();
            }
            if (!Sorting.Any())
            {
                foreach (var datatableSortingParam in datatableSortingParams)
                {
                    Sorting.Add(datatableSortingParam);
                }
            }
        }
    }

    [TypeConverter(typeof(DatatableOptionsConverter))]
    public class SortingOptions : List<DatatableSortingParam>
    {
        public SortingOptions() { }

        public SortingOptions(IEnumerable<DatatableSortingParam> collection)
            : base(collection) { }

        public SortingOptions Clone()
        {
            return new SortingOptions(this.Select(t => t.Clone()));
        }
    }

    [TypeConverter(typeof(DatatableOptionsConverter))]
    public class Filters : List<DatatableGroupFilterParam>
    {
        public Filters() { }

        public Filters(IEnumerable<DatatableGroupFilterParam> collection)
            : base(collection) { }

        public Filters Clone()
        {
            return CloneWithoutColumnFilter(null);
        }

        public Filters CloneWithoutColumnFilter(string columnName)
        {
            return new Filters(this.Select(t => t.CloneWithoutColumnFilter(columnName)));
        }
    }

    public class DatatableOptionsConverter : TypeConverter
    {
        private static readonly Type StringType = typeof(string);
        private readonly Type _targetType;

        public DatatableOptionsConverter(Type targetType)
        {
            _targetType = targetType;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == StringType)
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string str)
            {
                var obj = JsonConvert.DeserializeObject(str, _targetType);
                return obj;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
