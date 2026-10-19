
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UnnamedUiToolkit
{
    public static class CallbackExtensions
    {
        public static Expression<TD1> AppendCallback<TD1, TD2>(
            this Expression<TD1> Set,
            Expression<TD2> OnChange
            )
            where TD1 : Delegate
            where TD2 : Delegate
        {
            var parameterRewriter = new ParameterRewriter(new Dictionary<ParameterExpression, ParameterExpression>
            {
                { OnChange.Parameters[0], Set.Parameters[0] }
            });

            var concat = Set.Concat(OnChange, parameterRewriter);

            return concat;
        }
    }
}