using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnnamedUiToolkit
{
    public class MethodInfoParameterCache : DynamicDelegateHelper
    {
        public bool IsStatic => MethodInfo.IsStatic;
        public bool IsForSerializedProperty;
        public bool HasGUIStyle;
        public bool HasGUILayoutOption;
        public bool HasLabel;
        public bool AnyParamMatchReturnType;
        public bool HasCorrectNumberOfParams;
        public string Name => MethodInfo.Name;
        public readonly ParameterInfo[] ParameterInfo;

        public MethodInfoParameterCache(MethodInfo info)
            : base(info)
        {
            ParameterInfo = info.GetParameters();

            HasCorrectNumberOfParams = ParameterInfo.Length == 3;
            ProcessParams();
        }

        private void ProcessParams()
        {
            foreach (var i in ParameterInfo)
            {
                if (i.ParameterType == typeof(SerializedProperty)) IsForSerializedProperty = true;
                if (i.ParameterType == typeof(GUIStyle)) HasGUIStyle = true;
                if (i.ParameterType.HasElementType && i.ParameterType.GetElementType() == typeof(GUILayoutOption)) HasGUILayoutOption = true;
                if (i.ParameterType == typeof(string) && i.Name == "label") HasLabel = true;
                if (i.ParameterType == MethodInfo.ReturnType) AnyParamMatchReturnType = true;
            }
        }
    }
}