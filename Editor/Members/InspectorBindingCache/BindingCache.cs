
using System.Reflection;
using UnityEngine;

public delegate dynamic InspectorBindingDelegate(string label, dynamic value, GUILayoutOption[] layoyt);

namespace UnnamedUiToolkit
{
    public class DynamicDelegateHelper
    {
        public readonly MethodInfo MethodInfo;
        public DynamicDelegateHelper(MethodInfo info)
        {
            MethodInfo = info;
        }

        public InspectorBinding GetEditorBinding()
        {
            var expr = MethodInfo.CreateUniversalDelegate<InspectorBindingDelegate>(isWeakBind: true);

            var bind = new InspectorBinding()
            {
                MethodInfo = MethodInfo,
                Expresion = expr,
                Compiled = expr.Compile(),
            };

            return bind;
        }
    }

    public class InspectorBinding : DynamicCall<InspectorBindingDelegate>
    {
        public string Name => MethodInfo.Name;
        public MethodInfo MethodInfo { get; set; }
    }
}