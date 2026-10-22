using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public static class DynamicDelegateExtensions
    {
        public static Expression<TDelegateType> CreateLambda<TDelegateType>(this Expression body, IEnumerable<ParameterExpression> parameters)
            where TDelegateType : Delegate
        {
            var lambda = Expression.Lambda<TDelegateType>(
                body: body,
                tailCall: true,
                parameters: parameters
            );

            return lambda;
        }

        public static Expression<TDelegateType> CreateUniversalDelegate<TDelegateType>(this MethodInfo method, bool isWeakBind)
            where TDelegateType : Delegate
        {
            var lambdaParams = new List<ParameterExpression>();

            var callExpr = method.CreateUniversalDelegate(lambdaParams, isWeakBind);

            var lambda = callExpr.CreateLambda<TDelegateType>(lambdaParams);

            return lambda;
        }

        public static Expression CreateUniversalDelegate(this MethodInfo method, List<ParameterExpression> lambdaParams, bool isWeakBind)
        {
            Expression instanceExpr = null;

            var isStatic = method.IsStatic;

            if (!isStatic)
            {
                var declaring = method.DeclaringType;

                var instanceParam = Expression.Parameter(declaring, $"instance {declaring.Name}");

                lambdaParams.Add(instanceParam);

                instanceExpr = instanceParam;
            }

            // OtherKindOfUglyCastButLess
            var callArgs = ((IEnumerable<Expression>)method.BuildExpressionParameters(lambdaParams)).ToList();

            callArgs = callArgs.ConvertParamsWeakBind(lambdaParams, isWeakBind);

            if (!isStatic)
            {
                instanceExpr = callArgs[0];
                callArgs.RemoveAt(0);
            }

            var callExpr = Expression.Call(instanceExpr, method, callArgs) as Expression;

            callExpr = method.ConvertReturnWeakBind(callExpr, isWeakBind);

            return callExpr;
        }

        public static List<ParameterExpression> BuildExpressionParameters(this MethodInfo method, List<ParameterExpression> lambdaParams)
        {
            var methodParameters = method.GetParameters();

            for (int i = 0; i < methodParameters.Length; i++)
            {
                var p = methodParameters[i];

                var paramExpr = Expression.Parameter(p.ParameterType, p.Name);

                lambdaParams.Add(paramExpr);
            }

            return lambdaParams;
        }

        private static List<Expression> ConvertParamsWeakBind(this List<Expression> allParams, List<ParameterExpression> lambdaParams, bool isWeakBind)
        {
            if (!isWeakBind) return allParams;

            for (int i = 0; i < lambdaParams.Count; i++)
            {
                var original = lambdaParams[i];

                var weak = Expression.Parameter(typeof(object), original.Name);

                lambdaParams[i] = weak;

                var convert = Expression.Convert(weak, original.Type);

                allParams[i] = convert;
            }

            return allParams;
        }

        private static Expression ConvertReturnWeakBind(this MethodInfo MethodInfo, Expression call, bool isWeakBind)
        {
            if (!isWeakBind) return call;

            var rt = MethodInfo.ReturnType;

            if (rt == typeof(void)) return call;

            if (!rt.IsValueType) return call;

            call = Expression.Convert(call, typeof(object));

            return call;
        }
    }
}