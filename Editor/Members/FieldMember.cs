using System;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public class UiToolkitFieldMember : UiToolkitValueMember
    {
        public override FieldInfo BackingField { get; }
        public override Type MemberType { get; }
        public override DynamicCall<GetDelegate> GetExpression { get; protected set; }
        public override DynamicCall<SetDelegate> SetExpression { get; protected set; }
        public UiToolkitFieldMember(FieldInfo MemberInfo, InspectableMemberExtras InspectableMemberExtras)
            : base(MemberInfo, InspectableMemberExtras)
        {
            BackingField = MemberInfo;

            MemberType = MemberInfo.FieldType;

            SetExpression = new ()
            {
                Expresion = BackingField.CreateSetDelegate<SetDelegate>(),
            };

            GetExpression = new()
            {
                Expresion = BackingField.CreateGetDelegate<GetDelegate>(),
            };

            FinishInitialization();
        }
    }
}