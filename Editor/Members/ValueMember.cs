using System;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public abstract class UiToolkitValueMember :
        UiToolkitValueMemberInternal,
        IUiToolkitValueMember,
        IOnChangeMember
    {
        public OnChangeAttribute OnChange { get; protected set; }
        public DynamicCall<OnChange> OnChangeExpr { get; protected set; }
        public override BaseVisitorTarget VisitorTarget { get; protected set; }

        public UiToolkitValueMember(MemberInfo MemberInfo, InspectableMemberExtras InspectableMemberExtras)
               : base(MemberInfo, InspectableMemberExtras)
        {
            // Needs to be called after Membertype is defined;
            // CreateCachedDrawer();
        }

        public override NodeVisitor Draw(NodeVisitor Visitor)
        {
            VisitorTarget.RunNode(Visitor);

            return Visitor;
        }

        protected void FinishInitialization()
        {
            ProcessGetCall();
            ProcessOnChange();
            ProcessSetCall();
            CreateCachedDrawer();
        }

        protected void ProcessGetCall()
        {
            GetExpression.Compile();
        }

        protected void ProcessOnChange()
        {
            if (OnChange is null)
            {
                return;
            }

            var expr = OnChange.ProcessOnChangeAttribute(MemberInfo);

            OnChangeExpr = new()
            {
                Expresion = expr,
            };
        }

        protected void ProcessSetCall()
        {
            if (OnChangeExpr != null)
            {
                var original = SetExpression.Expresion;

                var chained = original.AppendCallback(OnChangeExpr.Expresion);

                SetExpression.Expresion = chained;
            }

            SetExpression.Compile();
        }

        protected void CreateCachedDrawer()
        {
            if (MemberType.IsEnum())
            {
                VisitorTarget = new EnumEditor(this);
                return;
            }

            if (MemberType.IsArray())
            {
                VisitorTarget = new ArrayEditor(this);
                return;
            }

            if (MemberType.IsPrimitive(out var binding))
            {
                VisitorTarget = new PrimitiveEditor(this, binding);
                return;
            }

            if (MemberType.IsChild())
            {
                VisitorTarget = new ChildEditor(this);
                return;
            }

            if (MemberType.IsUnityObject())
            {
                VisitorTarget = new NativeUnityObjectEditor(this);
                return;
            }

            throw new Exception("This Should Not Happen");
        }

        protected override void ProcessAttribute(Attribute Attribute)
        {
            if (Attribute is OnChangeAttribute OnChange)
            {
                // Not initialized here because lifecicle 
                // and constructor hell :)
                this.OnChange = OnChange;
                return;
            }

            base.ProcessAttribute(Attribute);
        }
    }

    public abstract class UiToolkitValueMemberInternal :
    UiToolkitMember,
    IUiToolkitDraw,
    IUiToolkitValueMember
    {
        public string RelativePath => BackingField.Name;
        public abstract FieldInfo BackingField { get; }
        public abstract Type MemberType { get; }
        public abstract DynamicCall<GetDelegate> GetExpression { get; }
        public abstract DynamicCall<SetDelegate> SetExpression { get; }
        public abstract BaseVisitorTarget VisitorTarget { get; protected set; }

        public bool IsRuntimeOnly { get; protected set; }

        protected UiToolkitValueMemberInternal(MemberInfo MemberInfo, InspectableMemberExtras InspectableMemberExtras)
            : base(MemberInfo, InspectableMemberExtras) { }

        protected override void ProcessAttribute(Attribute Attribute)
        {
            if (Attribute is RuntimeOnlyAttribute _)
            {
                IsRuntimeOnly = true;

                return;
            }

            base.ProcessAttribute(Attribute);
        }

        public abstract NodeVisitor Draw(NodeVisitor Visitor);
    }
}