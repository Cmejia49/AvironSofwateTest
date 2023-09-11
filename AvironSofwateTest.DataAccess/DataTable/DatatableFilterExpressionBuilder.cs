
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.DataAccess.DataTable
{
    public static class DatatableFilterExpressionBuilder
    {
        public static Expression<Func<T, bool>> BuildWhereClause<T>(IEnumerable<DatatableGroupFilterParam> filters)
        {
            return BuildWhereClause<T>(filters, string.Empty);
        }

        public static Expression<Func<T, bool>> BuildWhereClause<T>(IEnumerable<DatatableGroupFilterParam> groupFilters, string columnMapPrefix)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            Expression totalExpression = null;
            if (groupFilters != null)
            {
                foreach (DatatableGroupFilterParam groupFilter in groupFilters)
                {
                    Expression body = BuildFiltersWhereClause(groupFilter.Filters, columnMapPrefix, parameter);
                    totalExpression = totalExpression == null
                        ? body.Expand()
                        : AddExpression(totalExpression, body.Expand(), groupFilter.LogicalOperator);
                }
            }
            if (totalExpression == null)
                totalExpression = Expression.Constant(true, typeof(bool));

            return Expression.Lambda<Func<T, bool>>(totalExpression, parameter);
        }

        public static Expression CreatePropertyExpression(Expression parameter, string propertyName)
        {
            Expression body = parameter;
            foreach (string member in propertyName.Split('.'))
                body = Expression.PropertyOrField(body, member);

            return body;
        }

        private static Expression BuildFiltersWhereClause(IEnumerable<DatatableFilterParam> filters, string columnMapPrefix,
            ParameterExpression parameter)
        {
            Expression body = null;
            if (filters == null)
            {
                filters = Array.Empty<DatatableFilterParam>();
            }

            foreach (DatatableFilterParam filter in filters)
            {
                Expression filterBody = GetFilterBodyExpression(filter, parameter, columnMapPrefix);

                if (filterBody != null)
                {
                    body = body == null ? filterBody : AddExpression(body, filterBody, filter.LogicalOperator);
                }
            }

            return body ?? Expression.Constant(true, typeof(bool));
        }

        private static Expression GetFilterBodyExpression(DatatableFilterParam filter, ParameterExpression parameter, string columnMapPrefix)
        {
            Expression member = CreatePropertyExpression(parameter, columnMapPrefix + filter.ColumnName);
            Expression filterBody;

            Type sourceNullable = Nullable.GetUnderlyingType(member.Type);

            if (member.Type == typeof(string))
            {
                filterBody = GetStringExpressionBody(filter, member);
            }
            else if (member.Type == typeof(Guid))
            {
                filterBody = GetGuidExpressionBody(filter, member);
            }
            else if (member.Type == typeof(string[]))
            {
                filterBody = GetStringArrayExpressionBody(filter, member);
            }
            else if (new[] { typeof(int), typeof(int?) }.Contains(member.Type))
            {
                filterBody = GetNumericExpressionBody(filter, member, Convert.ToInt32);
            }
            else if (new[] { typeof(long), typeof(long?) }.Contains(member.Type))
            {
                filterBody = GetNumericExpressionBody(filter, member, Convert.ToInt64);
            }
            else if (new[] { typeof(decimal), typeof(decimal?) }.Contains(member.Type))
            {
                filterBody = GetNumericExpressionBody(filter, member, Convert.ToDecimal);
            }
            else if (new[] { typeof(byte), typeof(byte?) }.Contains(member.Type))
            {
                filterBody = GetNumericExpressionBody(filter, member, Convert.ToByte);
            }
            else if (new[] { typeof(double), typeof(double?) }.Contains(member.Type))
            {
                filterBody = GetNumericExpressionBody(filter, member, Convert.ToDouble);
            }
            else if (new[] { typeof(DateTime), typeof(DateTime?), typeof(DateTimeOffset), typeof(DateTimeOffset?) }.Contains(member.Type))
            {
                filterBody = GetDateExpressionBody(filter, member);
            }
            else if (new[] { typeof(bool), typeof(bool?) }.Contains(member.Type))
            {
                filterBody = GetBooleanExpressionBody(filter, member);
            }
            else if (member.Type.IsEnum || (sourceNullable != null && sourceNullable.IsEnum))
            {
                filterBody = GetEnumExpressionBody(filter, member);
            }
            else
            {
                throw new NotSupportedException($"Type '{member.Type}' is not supported.");
            }

            return filterBody;
        }

        private static Expression AddExpression(Expression expression1, Expression expression2, string logicalOperator)
        {
            if (logicalOperator == null || logicalOperator == DatatableFilterOptionConst.And)
            {
                return Expression.And(expression1, expression2);
            }

            if (logicalOperator == DatatableFilterOptionConst.Or)
            {
                return Expression.Or(expression1, expression2);
            }

            throw new NotSupportedException($"Operator '{logicalOperator}' is not supported");
        }

        private static Expression GetStringExpressionBody(DatatableFilterParam filter, Expression member)
        {
            Expression constant = Expression.Constant(filter.Value);
            MethodInfo containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
            MethodInfo startsWithMethod = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) });
            MethodInfo endsWithMethod = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) });

            return filter.Operator switch
            {
                DatatableFilterOptionConst.Contains => Expression.Call(member, containsMethod, constant),
                DatatableFilterOptionConst.DoesNotHave => Expression.Or(Expression.Not(Expression.Call(member, containsMethod, constant)), Expression.Equal(member, Expression.Constant(null))),
                DatatableFilterOptionConst.StartsWith => Expression.Call(member, startsWithMethod, constant),
                DatatableFilterOptionConst.EndsWith => Expression.Call(member, endsWithMethod, constant),
                DatatableFilterOptionConst.EqualTo => Expression.Equal(member, constant),
                DatatableFilterOptionConst.NotEqualTo => Expression.NotEqual(member, constant),
                DatatableFilterOptionConst.IsEmpty => Expression.Or(Expression.Equal(member, Expression.Constant(null)), Expression.Equal(member, Expression.Constant(string.Empty))),
                _ => throw new NotSupportedException($"Operator '{filter.Operator}' is not supported."),
            };
        }

        private static Expression GetGuidExpressionBody(DatatableFilterParam filter, Expression member)
        {
            _ = Guid.TryParse(filter.Value, out Guid g);

            Expression constant = Expression.Constant(g);

            return filter.Operator switch
            {
                DatatableFilterOptionConst.EqualTo => Expression.Equal(member, constant),
                DatatableFilterOptionConst.NotEqualTo => Expression.NotEqual(member, constant),
                DatatableFilterOptionConst.IsEmpty => Expression.Equal(member, Expression.Constant(Guid.Empty)),
                _ => throw new NotSupportedException($"Operator '{filter.Operator}' is not supported."),
            };
        }

        private static Expression GetStringArrayExpressionBody(DatatableFilterParam filter, Expression member)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(string), "x");
            var predicate = Expression.Lambda<Func<string, bool>>(GetStringExpressionBody(filter, parameter), parameter);

            return filter.Operator switch
            {
                DatatableFilterOptionConst.Contains => Expression.Call(typeof(Enumerable), "Any", new[] { typeof(string) }, member, predicate),
                DatatableFilterOptionConst.DoesNotHave => Expression.Call(typeof(Enumerable), "All", new[] { typeof(string) }, member, predicate),
                DatatableFilterOptionConst.EqualTo => Expression.Call(typeof(Enumerable), "Any", new[] { typeof(string) }, member, predicate),
                DatatableFilterOptionConst.IsEmpty => Expression.Or(Expression.Equal(member, Expression.Constant(null)), Expression.Equal(Expression.Constant(0), Expression.ArrayLength(member))),
                _ => throw new NotSupportedException($"Operator '{filter.Operator}' is not supported."),
            };
        }

        private static bool IsNullable(Expression expression)
        {
            Type type = expression.Type;
            return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }

        private static Expression GetNumericExpressionBody<T>(DatatableFilterParam filter, Expression member,
            Func<string, IFormatProvider, T> convert)
            where T : struct
        {
            if (filter.Operator == DatatableFilterOptionConst.Contains)
                return GetContainsExpressionBody(filter, member);

            if (filter.Operator == DatatableFilterOptionConst.Between)
                return GetBetweenExpressionBody(filter, member, convert);

            var constant = GetNullabelConstant(filter.Value, member, convert);

            return filter.Operator switch
            {
                DatatableFilterOptionConst.EqualTo => Expression.Equal(member, constant),
                DatatableFilterOptionConst.NotEqualTo => Expression.NotEqual(member, constant),
                DatatableFilterOptionConst.GreaterThan => Expression.GreaterThan(member, constant),
                DatatableFilterOptionConst.LessThan => Expression.LessThan(member, constant),
                DatatableFilterOptionConst.GreaterThanOrEqual => Expression.GreaterThanOrEqual(member, constant),
                DatatableFilterOptionConst.LessThanOrEqual => Expression.LessThanOrEqual(member, constant),
                DatatableFilterOptionConst.IsEmpty => IsNullable(member)
                    ? Expression.Equal(member, Expression.Constant(null))
                    : Expression.Equal(Expression.Constant(true), Expression.Constant(false)),
                _ => throw new NotSupportedException($"Operator '{filter.Operator}' is not supported."),
            };
        }

        private static Expression GetNullabelConstant<T>(string value, Expression member,
            Func<string, IFormatProvider, T> convert)
            where T : struct
        {
            var isNullable = IsNullable(member);
            T? convertedValue = isNullable && value == null
                ? default
                : (T?)convert(value, CultureInfo.InvariantCulture);

            Expression constant = isNullable
                ? Expression.Convert(Expression.Constant(convertedValue), member.Type)
                : Expression.Constant(convertedValue);

            return constant;
        }

        private static Expression GetBetweenExpressionBody<T>(DatatableFilterParam filter, Expression member,
            Func<string, IFormatProvider, T> convert)
            where T : struct
        {
            string[] values = GetBetweenValue(filter);
            if (values == null) return null;
            var constantA = GetNullabelConstant(values[0], member, convert);
            var constantB = GetNullabelConstant(values[1], member, convert);

            return Expression.And(
                Expression.GreaterThanOrEqual(member, constantA),
                Expression.LessThanOrEqual(member, constantB));
        }

        private static Expression GetBooleanExpressionBody(DatatableFilterParam filter, Expression member)
        {
            if (filter.Operator == DatatableFilterOptionConst.Contains)
                return GetContainsExpressionBody(filter, member);

            bool? value = filter.Value == null ? default(bool?) : bool.Parse(filter.Value);
            Expression constant = IsNullable(member)
                ? Expression.Convert(Expression.Constant(value), member.Type)
                : Expression.Constant(value);

            return filter.Operator switch
            {
                DatatableFilterOptionConst.EqualTo or DatatableFilterOptionConst.IsFalse or DatatableFilterOptionConst.IsTrue => Expression.Equal(member, constant),
                DatatableFilterOptionConst.NotEqualTo => Expression.NotEqual(member, constant),
                _ => throw new NotSupportedException($"Operator '{filter.Operator}' is not supported."),
            };
        }

        private static Expression GetEnumExpressionBody(DatatableFilterParam filter, Expression member)
        {
            if (filter.Operator == DatatableFilterOptionConst.Contains)
                return GetContainsExpressionBody(filter, member);

            var sourceType = member.Type;

            var parsedNullable = Nullable.GetUnderlyingType(sourceType);
            if (parsedNullable != null && parsedNullable.IsEnum)
                sourceType = parsedNullable;

            object value = Enum.Parse(sourceType, filter.Value);

            Expression constant = IsNullable(member)
                ? Expression.Convert(Expression.Constant(value), member.Type)
                : Expression.Constant(value);

            return filter.Operator switch
            {
                DatatableFilterOptionConst.EqualTo => Expression.Equal(member, constant),
                DatatableFilterOptionConst.NotEqualTo => Expression.NotEqual(member, constant),
                _ => throw new NotSupportedException($"Operator '{filter.Operator}' is not supported."),
            };
        }

        private static Expression GetDateExpressionBody(DatatableFilterParam filter, Expression member)
        {
            var isNullable = IsNullable(member);
            var memberType = isNullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset);
            member = Expression.Convert(member, memberType);

            if (filter.Operator == DatatableFilterOptionConst.Between)
                return GetDateBetweenExpressionBody(filter, member, memberType);

            var constant = GetDateConstant(filter.Value, memberType);

            return filter.Operator switch
            {
                DatatableFilterOptionConst.EqualTo => filter.DatePart != null
                    ? GetDateDiffExpressionBody(member, constant, filter.DatePart)
                    : GetDateDiffExpressionBody(member, constant),
                DatatableFilterOptionConst.NotEqualTo => Expression.NotEqual(member, constant),
                DatatableFilterOptionConst.GreaterThan => Expression.GreaterThan(member, constant),
                DatatableFilterOptionConst.LessThan => Expression.LessThan(member, constant),
                DatatableFilterOptionConst.GreaterThanOrEqual => Expression.GreaterThanOrEqual(member, constant),
                DatatableFilterOptionConst.LessThanOrEqual => Expression.LessThanOrEqual(member, constant),
                DatatableFilterOptionConst.IsEmpty => isNullable
                    ? Expression.Equal(member, Expression.Convert(Expression.Constant(null), typeof(DateTimeOffset?)))
                    : Expression.Equal(Expression.Constant(true), Expression.Constant(false)),
                _ => throw new NotSupportedException($"Operator '{filter.Operator}' is not supported."),
            };
        }

        private static Expression GetDateConstant(string value, Type memberType)
        {
            DateTimeOffset? dt = value == null
                            ? null
                            : DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            return Expression.Convert(Expression.Constant(dt), memberType);
        }

        private static Expression GetDateBetweenExpressionBody(DatatableFilterParam filter, Expression member, Type memberType)
        {
            string[] values = GetBetweenValue(filter);
            if (values == null) return null;
            var constantA = GetDateConstant(values[0], memberType);
            var constantB = GetDateConstant(values[1], memberType);

            return Expression.And(
                Expression.GreaterThanOrEqual(member, constantA),
                Expression.LessThanOrEqual(member, constantB));
        }

        private static string[] GetBetweenValue(DatatableFilterParam filter)
        {
            var values = filter.Value?.Split(",,");
            if (values != null && (!filter.Value.Contains(",,") || values.Length != 2))
            {
                throw new NotSupportedException($"Value must have two parts, separated with ',,'. ({filter.Value})");
            }
            // If the left value of ",," (start date) is empty, set a default date for values[0]
            if (string.IsNullOrWhiteSpace(values[0]))
            {
                values[0] = "8/15/1909, 12:00:00 AM";
            }
            // If the right value of ",," (end date) is empty, set a default date for values[1]
            if (string.IsNullOrWhiteSpace(values[1]))
            {
                values[1] = "8/15/2099, 12:00:00 AM";
            }
            return values;
        }

        private static Expression GetContainsExpressionBody(DatatableFilterParam filter, Expression member)
        {
            MethodInfo toString = typeof(object).GetMethod("ToString");
            return GetStringExpressionBody(filter, Expression.Call(member, toString));
        }

        private static Expression GetDateDiffExpressionBody(Expression member, Expression constant, string datepart = "second")
        {
            var methodName = "DateDiff" + datepart;

            MethodInfo method = typeof(SqlServerDbFunctionsExtensions)
                .GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(DbFunctions), typeof(DateTimeOffset?), typeof(DateTimeOffset?) }, null);

            if (method == null)
            {
                throw new NotSupportedException(methodName);
            }

            MemberExpression arg0 = Expression.Property(null, typeof(EF), nameof(EF.Functions));
            Expression arg1 = ToNullable<DateTimeOffset>(member);
            Expression arg2 = ToNullable<DateTimeOffset>(constant);

            Expression dateDiffResult = Expression.Call(null, method, arg0, arg1, arg2);

            return Expression.Equal(dateDiffResult, Expression.Constant(0, typeof(int?)));
        }

        private static Expression ToNullable<T>(Expression expression) where T : struct
        {
            return Expression.Convert(expression, typeof(T?));
        }
    }
}
