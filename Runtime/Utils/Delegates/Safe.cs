using System;
using Newtonsoft.Json;
using UnityEngine;

namespace UnnamedUiToolkit
{
    public static class Safe
    {
#warning Maybe Do Optimized Path Here For Non Debug Cases
        public static object DebugCall(this Delegate predicate, params object[] args)
        {
            try
            {
                var retval = predicate.DynamicInvoke(args);

                return retval;
            }
            catch (Exception ex)
            {
                DelegateExceptionParser(predicate, ex);

                throw;
            }

            return null;
        }

        public static void DelegateExceptionParser(Delegate predicate, Exception exception)
        {
            var isStatic = !predicate.TryGetInstance(out var target, out var targetType, out var isClosure);

            object serializedInstance = target;

#if NewtonsoftJsonAvailable

            serializedInstance = JsonConvert.SerializeObject(value: target, settings: CachedSettings);

#endif

            var message =
                $@"
Error calling delegate:
IsStatic => {isStatic}
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