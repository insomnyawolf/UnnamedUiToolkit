using System;
using System.Linq.Expressions;

namespace UnnamedUiToolkit
{
    public class DynamicCall<TDelegate>
        where TDelegate : Delegate
    {
        public Expression<TDelegate> Expresion { get; set; }
        public TDelegate Compiled { get; set; }
        public void Compile()
        {
            Compiled = Expresion.Compile();
        }

        public object CompiledDebug(params object[] args)
        {
            try
            {
                var retval = Compiled.DebugCall(args);

                return retval;
            } 
            catch (Exception _)
            {
                throw;
            }

            return null;
        }
    }
}