using System;
using System.Linq.Expressions;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public static class AccessorExtensions
    {
        // Those last two are not good enough

        public static Expression<TDelegateType> CreateSetDelegate<TDelegateType>(this FieldInfo fieldInfo)
            where TDelegateType : Delegate
        {
            var instanceParam = Expression.Parameter(typeof(object), "instance");
            var convertInstance = Expression.Convert(instanceParam, fieldInfo.DeclaringType);

            var valueParam = Expression.Parameter(typeof(object), "nameof(TValue)");
            var convertValue = Expression.Convert(valueParam, fieldInfo.FieldType);

            var fieldExpr = Expression.Field(convertInstance, fieldInfo);
            var assignExpr = Expression.Assign(fieldExpr, convertValue);

            var lambda = assignExpr.CreateLambda<TDelegateType>(new[] { instanceParam, valueParam });

            return lambda;
        }

        public static Expression<TDelegateType> CreateGetDelegate<TDelegateType>(this FieldInfo fieldInfo)
            where TDelegateType : Delegate
        {
            var instanceParam = Expression.Parameter(typeof(object), "instance");
            var convertInstance = Expression.Convert(instanceParam, fieldInfo.DeclaringType);

            var fieldExpr = Expression.Field(convertInstance, fieldInfo);
            var convertedField = Expression.Convert(fieldExpr, typeof(object));

            var lambda = convertedField.CreateLambda<TDelegateType>(new[] { instanceParam });

            return lambda;
        }
    }
}