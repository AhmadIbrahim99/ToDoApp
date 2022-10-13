using System.Linq;
using System.Linq.Expressions;

namespace ClassLibrary.Common.Extenstions
{
    public static class OrderExtensions
    {
        private static IOrderedQueryable<T>  OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), string.Empty);
            MemberExpression property = Expression.PropertyOrField(parameter, propertyName);
            LambdaExpression sort = Expression.Lambda(property, parameter);
            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy")+ (descending ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type},
                source.Expression,
                Expression.Quote(sort));
            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName) => OrderingHelper<T>(source,propertyName, false, false);

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName) => OrderingHelper<T>(source, propertyName, true, false);

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName) => OrderingHelper<T>(source, propertyName, false, true);
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName) => OrderingHelper<T>(source, propertyName, true, true);

    }
}
