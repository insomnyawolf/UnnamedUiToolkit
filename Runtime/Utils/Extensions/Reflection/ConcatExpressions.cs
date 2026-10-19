using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public static class ExpressionConcat
{
    public static Expression<TD1> Concat<TD1, TD2>(
        this Expression<TD1> Main,
        Expression<TD2> Append,
        ParameterRewriter parameterRewriter = null
        )
        where TD1 : Delegate
        where TD2 : Delegate
    {
        // No callback to append
        if (Append == null) return Main;

        parameterRewriter ??= new();

        var rewrittenOnChangeBody = parameterRewriter.Visit(Append.Body);

        var setRt = Main.ReturnType;

        var isVoid = setRt == typeof(void);

        Expression body;

        if (isVoid)
        {
            body = Expression.Block(
                type:setRt, 
                Main.Body, 
                rewrittenOnChangeBody
            );
        }
        else
        {
            var retVal = Expression.Variable(setRt, "retVal");

            body = Expression.Block(
                type: setRt,
                variables: new[] { retVal },
                Expression.Assign(retVal, Main.Body),
                rewrittenOnChangeBody,
                retVal
            );
        }

        var lambda = Expression.Lambda<TD1>(
            body: body,
            tailCall: Main.TailCall,
            parameters: Main.Parameters
        );

        return lambda;
    }
}

public class ParameterRewriter : ExpressionVisitor
{
    private readonly Dictionary<ParameterExpression, ParameterExpression> Map;

    /// <summary>
    /// Key: Parameter To Replace
    /// Value: Replacement
    /// </summary>
    /// <param name="map"></param>
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