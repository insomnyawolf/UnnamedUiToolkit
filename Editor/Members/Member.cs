using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace UnnamedUiToolkit
{
    public abstract class UiToolkitMember : UiToolkitElement, IUiToolkitMember
    {
        public string ReflectedName { get; protected set; }
        public MemberInfo MemberInfo { get; protected set; }

        protected UiToolkitMember(MemberInfo MemberInfo, InspectableMemberExtras InspectableMemberExtras)
            : base(InspectableMemberExtras)
        {
            this.MemberInfo = MemberInfo;
            
            ReflectedName = MemberInfo.Name;

            DisplayName ??= ObjectNames.NicifyVariableName(ReflectedName);
        }
    }
}