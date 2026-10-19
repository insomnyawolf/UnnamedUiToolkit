using System;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public class UiToolkitFieldMember : UiToolkitValueMember
    {
        public override FieldInfo BackingField { get; }
        public override Type MemberType { get; }
        public override DynamicCall<GetDelegate> GetExpression { get; }
        public override DynamicCall<SetDelegate> SetExpression { get; }
        public UiToolkitFieldMember(FieldInfo MemberInfo, InspectableMemberExtras InspectableMemberExtras)
            : base(MemberInfo, InspectableMemberExtras)
        {
            BackingField = MemberInfo;

            MemberType = MemberInfo.FieldType;

            CreateCachedDrawer();

            SetExpression = new ()
            {
                Expresion = BackingField.CreateSetDelegate<SetDelegate>(),
            };

            SetExpression.Compile();

            GetExpression = new()
            {
                Expresion = BackingField.CreateGetDelegate<GetDelegate>(),
            };

            GetExpression.Compile();
        }
    }
}