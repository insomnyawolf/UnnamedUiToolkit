
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UnnamedUiToolkit
{
    public static class CallbackExtensions
    {
        public static Expression<Delegate> AppendCallback<Delegate>(
            Expression<Delegate> setter,
            Expression<Delegate> onChange
       )
        {
            var parameters = setter.Parameters;

            var parameterRewriter = new ParameterRewriter(new Dictionary<ParameterExpression, ParameterExpression>
            {
                { onChange.Parameters[0], parameters[0] }
            });

            var concat = setter.Concat(onChange, parameterRewriter);

            return concat;
        }
    }
}