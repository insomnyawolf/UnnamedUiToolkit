using System;
using System.Linq.Expressions;
using Newtonsoft.Json;
using UnityEngine;

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

#warning Maybe Do Optimized Path Here For Non Debug Cases
        public static void SafeCall(Action predicate)
        {
            try
            {
                predicate();
            }
            catch (Exception ex)
            {
                ExceptionManager(predicate, ex);

                throw;
            }
        }

        public static void ExceptionManager(Delegate predicate, Exception exception)
        {
            var isStatic = !predicate.TryGetInstance(out var target, out var targetType, out var isClosure);

            object serializedInstance = target;

#if NewtonsoftJsonAvailable

            serializedInstance = JsonConvert.SerializeObject(value: target,  settings: CachedSettings);

#endif

            var message =
                $@"
Error generating field:
IsClosure => {isClosure}
Instance => {target}
InstanceType => {targetType}
Exception => {exception}
            ";

            Debug.LogError(message);
        }

#if NewtonsoftJsonAvailable

            private static readonly JsonSerializerSettings CachedSettings = new()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
            };

#endif

    }
}