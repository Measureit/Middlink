using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Middlink.Core.Extensions
{
    public static partial class Extensions
    {
        public static Guid ToGuid(this string from)
        {
            Guid result;
            Guid.TryParse(from, out result);
            return result;
        }

        public static string Underscore(this string value) => string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
        public static T Bind<T>(this T model, Expression<Func<T, object>> expression, object value) => model.Bind<T, object>(expression, value);

        public static T BindId<T>(this T model, Expression<Func<T, Guid>> expression) => model.Bind(expression, Guid.NewGuid());

        public static T Bind<T, TFrom, TTo>(this T model, Expression<Func<T, TTo>> expression, TFrom value, Func<TFrom, TTo> converter)
            => model.Bind<T, TTo>(expression, converter(value));

        private static TModel Bind<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression, object value)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            var propertyName = memberExpression.Member.Name.ToLowerInvariant();
            var modelType = model.GetType();
            var field = modelType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .SingleOrDefault(x => x.Name.ToLowerInvariant().StartsWith($"<{propertyName}>"));
            if (field == null)
            {
                return model;
            }

            field.SetValue(model, value);

            return model;
        }
    }
}
