
using System;

namespace UnnamedUiToolkit
{
    public class PrimitiveEditor : DrawerVisitorTarget
    {
        // Simple drawer mapping based on type
        public static bool IsTarget(Type MemberType, out InspectorBinding binding)
        {
            binding = null;

            if (MemberType.IsEnum)
            {
                return false;
            }

            if (MemberType.IsArray)
            {
                return false;
            }

            if (MemberType.IsUnityNativeType())
            {
                return false;
            }

            if (!InspectorBindingCache.PropertyDrawersCache.TryGetValue(MemberType, out binding))
            {
                return false;
            }

            return true;
        }

        protected Func<object, object> CachedDrawer { get; set; }
        protected InspectorBinding InspectorBinding { get; set; }

        public PrimitiveEditor(IUiToolkitValueMember Member, InspectorBinding InspectorBinding)
            : base(Member)
        {
            this.InspectorBinding = InspectorBinding;

            // IdkHow To Avoid The Capturing
            object DrawPrimitive(object value)
            {
                var val = InspectorBinding.Compiled(Member.DisplayName, value, EmptyGuiLayoutOptions);

                return val;
            }

            CachedDrawer = DrawPrimitive;
        }

        public override DrawerVisitor RunNode(DrawerVisitor Visitor)
        {
            var values = Visitor.Values;

            var newVal = CachedDrawer(values.Value);

            values.Update(newVal);

            return Visitor;
        }
    }
}