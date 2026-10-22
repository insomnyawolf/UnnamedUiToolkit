using System;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public class UiToolkitPropertyMember : UiToolkitValueMember
    {
        public override FieldInfo BackingField { get; }
        public override Type MemberType { get; }
        public override DynamicCall<GetDelegate> GetExpression { get; protected set; }
        public override DynamicCall<SetDelegate> SetExpression { get; protected set; }

        public UiToolkitPropertyMember(PropertyInfo MemberInfo, FieldInfo BackingField, InspectableMemberExtras InspectableMemberExtras)
            : base(MemberInfo, InspectableMemberExtras)
        {
            MemberType = MemberInfo.PropertyType;

            this.BackingField = BackingField;

            var accesors = MemberInfo.GetAccessors(nonPublic: true);

            foreach (var accessor in accesors)
            {
                // Setter is the one that doesn't return

                if (accessor.ReturnType == typeof(void))
                {
                    SetExpression = new () 
                    {
                        Expresion = accessor.CreateUniversalDelegate<SetDelegate>(isWeakBind: true),
                    };
                }
                else
                {
                    GetExpression = new ()
                    {
                        Expresion = accessor.CreateUniversalDelegate<GetDelegate>(isWeakBind: true),
                    };
                }
            }

            FinishInitialization();
        }
    }
}