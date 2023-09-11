using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.DataAccess.DataTable
{
    public static class DatatableQueryableExtensions
    {
        private static readonly MethodInfo OrderByMethod = typeof(Queryable).GetMethods()
            .Single(method => method.Name == "OrderBy" && method.GetParameters().Length == 2);

        private static readonly MethodInfo OrderByDescendingMethod = typeof(Queryable).GetMethods()
            .Single(method => method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

        private static readonly MethodInfo OrderByThenMethod = typeof(Queryable).GetMethods()
            .Single(method => method.Name == "ThenBy" && method.GetParameters().Length == 2);

        private static readonly MethodInfo OrderByThenDescendingMethod = typeof(Queryable).GetMethods()
            .Single(method => method.Name == "ThenByDescending" && method.GetParameters().Length == 2);

        private static IQueryable<T> OrderByProperty<T>(IQueryable<T> source, string propertyName, bool descending)
        {
            MethodInfo targetMethod = descending ? OrderByDescendingMethod : OrderByMethod;

            return ApplyOrderBy(source, propertyName, targetMethod);
        }

        private static IQueryable<T> OrderThenByProperty<T>(IQueryable<T> source, string propertyName, bool descending)
        {
            MethodInfo targetMethod = descending ? OrderByThenDescendingMethod : OrderByThenMethod;

            return ApplyOrderBy(source, propertyName, targetMethod);
        }

        private static IQueryable<T> ApplyOrderBy<T>(IQueryable<T> source, string propertyName, MethodInfo method)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T));

            Expression orderByProperty = DatatableFilterExpressionBuilder
                .CreatePropertyExpression(parameterExpression, propertyName);
            LambdaExpression lambda = Expression.Lambda(orderByProperty, parameterExpression);

            MethodInfo genericMethod = method.MakeGenericMethod(typeof(T), orderByProperty.Type);
            var ret = genericMethod.Invoke(null, new object[] { source, lambda });
            return (IQueryable<T>)ret;
        }

        public static IQueryable<T> OrderBySortingOptions<T>(this IQueryable<T> source, SortingOptions sorting)
        {
            if (sorting == null || !sorting.Any())
                throw new ArgumentException("Filters must have a Sorting column");

            var hasOptionsWithEmptyColumnName = sorting.Any(so => string.IsNullOrEmpty(so.ColumnName));
            if (hasOptionsWithEmptyColumnName)
            {
                throw new ArgumentException($"{nameof(SortingOptions)}." +
                    $"{nameof(DatatableSortingParam.ColumnName)} could not be empty!");
            }

            DatatableSortingParam firstSortingOption = sorting.First();
            source = OrderByProperty(source, firstSortingOption.ColumnName, firstSortingOption.Descending);

            foreach (DatatableSortingParam sortingOption in sorting.Skip(1))
            {
                source = OrderThenByProperty(source, sortingOption.ColumnName, sortingOption.Descending);
            }

            return source;
        }

        public static IList<T> ToOrderPageList<T>(this IQueryable<T> source, DatatableQueryModel query)
        {
            return source.OrderBySortingOptions(query.Sorting).ToPageList(query);
        }

        public static IList<TSource> ToOrderPageList<TSource, TOrderBy>(this IQueryable<TSource> source,
            Expression<Func<TSource, TOrderBy>> orderExpression, DatatableQueryModel query)
        {
            if (orderExpression != null)
                source = source.OrderBy(orderExpression);

            return source.ToPageList(query);
        }

        public static Task<IEnumerable<TSource>> ToOrderPageListAsync<TSource>(this IQueryable<TSource> source,
           DatatableQueryModel query, CancellationToken cancellationToken)
        {
            return source.OrderBySortingOptions(query.Sorting).ToPageListAsync(query, cancellationToken);
        }

        public static Task<IEnumerable<TSource>> ToOrderPageListAsync<TSource, TOrderBy>(this IQueryable<TSource> source,
            Expression<Func<TSource, TOrderBy>> orderExpression, DatatableQueryModel query)
        {
            return source.ToOrderPageListAsync(orderExpression, query, CancellationToken.None);
        }

        public static Task<IEnumerable<TSource>> ToOrderPageListAsync<TSource, TOrderBy>(this IQueryable<TSource> source,
            Expression<Func<TSource, TOrderBy>> orderExpression, DatatableQueryModel query, CancellationToken cancellationToken)
        {
            if (orderExpression != null)
                source = source.OrderBy(orderExpression);

            return source.ToPageListAsync(query, cancellationToken);
        }

        public static IList<TSource> ToOrderDescendingPageList<TSource, TOrderBy>(this IQueryable<TSource> source,
            Expression<Func<TSource, TOrderBy>> orderExpression, DatatableQueryModel query)
        {
            if (orderExpression != null)
                source = source.OrderByDescending(orderExpression);

            return source.ToPageList(query);
        }

        public static Task<IEnumerable<TSource>> ToOrderDescendingPageListAsync<TSource, TOrderBy>(this IQueryable<TSource> source,
            Expression<Func<TSource, TOrderBy>> orderExpression, DatatableQueryModel query)
        {
            return source.ToOrderDescendingPageListAsync(orderExpression, query, CancellationToken.None);
        }

        public static async Task<IEnumerable<TSource>> ToOrderDescendingPageListAsync<TSource, TOrderBy>(this IQueryable<TSource> source,
            Expression<Func<TSource, TOrderBy>> orderExpression, DatatableQueryModel query, CancellationToken cancellationToken)
        {
            if (orderExpression != null)
                source = source.OrderByDescending(orderExpression);

            return await source.ToPageListAsync(query, cancellationToken);
        }

        public static IList<TSource> ToPageList<TSource>(this IQueryable<TSource> source, DatatableQueryModel query)
        {
            if (query == null)
                return source.ToList();

            var take = query.PageSize;
            var skip = query.Page * query.PageSize;

            return take == 0
                ? source.ToList()
                : source.Skip(skip).Take(take).ToList();
        }

        public static async Task<IEnumerable<TSource>> ToPageListAsync<TSource>(this IQueryable<TSource> source,
            DatatableQueryModel query, CancellationToken cancellationToken)
        {
            if (query == null)
                return await source.ToListAsync(cancellationToken);

            var take = query.PageSize;
            var skip = query.Page * query.PageSize;

            return take == 0
                ? await source.ToListAsync(cancellationToken)
                : await source.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        public static IQueryable<T> ApplyDatatableQuery<T>(this IQueryable<T> source, Filters filters)
        {
            if (filters == null)
                return source;

            Expression<Func<T, bool>> whereClause = DatatableFilterExpressionBuilder.BuildWhereClause<T>(filters);

            return source.Where(whereClause);
        }
    }

}
