namespace System.Linq.Dynamic
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    internal static class IQueryableUtil<T, PT>
    {
        public static IQueryable Contains(IQueryable Source, string PropertyName, string SearchClause)
        {
            ParameterExpression expression;
            MemberExpression expression2 = Expression.Property(expression = Expression.Parameter(typeof(T), "item"), PropertyName);
            Expression.Convert(expression2, typeof(object));
            ConstantExpression expression3 = Expression.Constant(SearchClause, typeof(string));
            Expression<Func<T, bool>> expression5 = Expression.Lambda<Func<T, bool>>(Expression.Call(expression2, "Contains", new Type[0], new Expression[] { expression3 }), new ParameterExpression[] { expression });
            return Queryable.Where<T>(Source.OfType<T>().AsQueryable<T>(), expression5);
        }

        public static IQueryable Sort(IQueryable source, string sortExpression, bool Ascending)
        {
            ParameterExpression expression;
            Expression<Func<T, PT>> expression2 = Expression.Lambda<Func<T, PT>>(Expression.Convert(Expression.Property(expression = Expression.Parameter(typeof(T), "item"), sortExpression), typeof(PT)), new ParameterExpression[] { expression });
            if (Ascending)
            {
                return Queryable.OrderBy<T, PT>(source.OfType<T>().AsQueryable<T>(), expression2);
            }
            return Queryable.OrderByDescending<T, PT>(source.OfType<T>().AsQueryable<T>(), expression2);
        }
    }
}

