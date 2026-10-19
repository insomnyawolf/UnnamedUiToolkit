using System.Collections.Generic;
using System.Linq.Expressions;

public static class ExpressionConcat
{
    public static Expression<Delegate> Concat<Delegate>(
        this Expression<Delegate> setter,
        Expression<Delegate> onChange,
        ParameterRewriter parameterRewriter = null
        )
    {
        // No callback to append
        if (onChange == null) return setter;

        parameterRewriter ??= new();

        var rewrittenOnChangeBody = parameterRewriter.Visit(onChange.Body);

        var setRt = setter.ReturnType;

        var isVoid = setRt == typeof(void);

        Expression body;

        if (isVoid)
        {
            body = Expression.Block(
                type:setRt, 
                setter.Body, 
                rewrittenOnChangeBody
            );
        }
        else
        {
            var retVal = Expression.Variable(setRt, "retVal");

            body = Expression.Block(
                type: setRt,
                variables: new[] { retVal },
                Expression.Assign(retVal, setter.Body),
                rewrittenOnChangeBody,
                retVal
            );
        }

        var lambda = Expression.Lambda<Delegate>(
            body: body,
            tailCall: setter.TailCall,
            parameters: setter.Parameters
        );

        return lambda;
    }
}

public class ParameterRewriter : ExpressionVisitor
{
    private readonly Dictionary<ParameterExpression, ParameterExpression> Map;

    public ParameterRewriter(Dictionary<ParameterExpression, ParameterExpression> map = null)
    {
        Map = map ?? new();
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (Map.TryGetValue(node, out var replacement))
        {
            return replacement;
        }

        return base.VisitParameter(node);
    }
}