using System;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public abstract class UiToolkitValueMember :
       UiToolkitValueMemberInternal<GetDelegate, SetDelegate>,
       IUiToolkitValueMember
    {
        public UiToolkitValueMember(MemberInfo MemberInfo, InspectableMemberExtras InspectableMemberExtras)
               : base(MemberInfo, InspectableMemberExtras)
        {
            // Needs to be called after Membertype is defined;
            // CreateCachedDrawer();
        }

        protected BaseVisitorTarget<DrawerVisitor> Cache;

        public override DrawerVisitor Draw(DrawerVisitor Visitor)
        {
            Cache.RunNode(Visitor);

            return Visitor;
        }

        protected void CreateCachedDrawer()
        {
            if (EnumEditor.IsTarget(MemberType))
            {
                Cache = new EnumEditor(this);
                return;
            }

            if (ArrayEditor.IsTarget(MemberType))
            {
                Cache = new ArrayEditor(this);
                return;
            }

            if (PrimitiveEditor.IsTarget(MemberType, out var binding))
            {
                Cache = new PrimitiveEditor(this, binding);
                return;
            }

            if (ChildEditor.IsTarget(MemberType))
            {
                Cache = new ChildEditor(this);
                return;
            }

            if (NativeUnityObjectEditor.IsTarget(MemberType))
            {
                Cache = new NativeUnityObjectEditor(this);
                return;
            }

            throw new Exception("This Should Not Happen");
        }
    }

    public abstract class UiToolkitValueMemberInternal<TGetDelegate, TSetDelegate> :
    UiToolkitMember,
    IOnChangeMember,
    IUiToolkitDraw,
    IUiToolkitValueMemberInternal<TGetDelegate, TSetDelegate>
        where TGetDelegate : Delegate
        where TSetDelegate : Delegate
    {
        public string RelativePath => BackingField.Name;
        public abstract FieldInfo BackingField { get; }
        public abstract Type MemberType { get; }
        public abstract DynamicCall<TGetDelegate> GetExpression { get; }
        public abstract DynamicCall<TSetDelegate> SetExpression { get; }

        public bool IsRuntimeOnly { get; protected set; }
        public DynamicCall<OnChange> OnChangeExpr { get; protected set; }


        protected UiToolkitValueMemberInternal(MemberInfo MemberInfo, InspectableMemberExtras InspectableMemberExtras)
            : base(MemberInfo, InspectableMemberExtras) { }

        protected override void ProcessAttribute(Attribute Attribute)
        {
            if (Attribute is OnChangeAttribute OnChange)
            {
                var expr = OnChange.ProcessOnChangeAttribute(MemberInfo);

                OnChangeExpr = new()
                {
                    Expresion = expr,
                };

                return;
            }

            if (Attribute is RuntimeOnlyAttribute _)
            {
                IsRuntimeOnly = true;

                return;
            }

            base.ProcessAttribute(Attribute);
        }

        public abstract DrawerVisitor Draw(DrawerVisitor Visitor);
    }
}