using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public class TreeHandler
    {
        protected readonly UnnamedUiToolkitAttribute[] Attributes;
        public bool IsValidRootTarget => Attributes.Length > 0;

        protected readonly Type TargetType;

        private readonly Dictionary<FieldInfo, InspectableMemberExtras> KnownMappings = new();
        private readonly List<IUiToolkitBase> Members = new();

        public TreeHandler(Type TargetType)
        {
            this.TargetType = TargetType;

            Attributes = LoadAttributes();
        }

        protected UnnamedUiToolkitAttribute[] LoadAttributes()
        {
            var attrEnumerable = TargetType.GetCustomAttributes<UnnamedUiToolkitAttribute>(inherit: true);

            var attrArray = attrEnumerable.ToArray();

            return attrArray;
        }

        public void VisitNodes(NodeVisitor NodeVisitor)
        {
            foreach (var member in Members)
            {
                NodeVisitor.Visit(member);
            }
        }

        public void Initialize()
        {
            // Temp until proper caching is made
            Members.Clear();

            var members = TargetType.GetMembers(GenericReflectionHelpers.ReflectionFlags)
                .Where(member => !member.DeclaringType.IsUnityNativeType())
            //.Reverse()
            //.OrderBy(OrderControls)
                .OrderBy(m => m.MetadataToken)
                .ToList()
            ;

            foreach (var member in members)
            {
                var Attributes = member.GetAllAttributres();

                if (Attributes.TryFindAttribute<SkipAttribute>(out var _))
                {
                    // Literally do nothing
                    continue;
                }

                var extra = new InspectableMemberExtras()
                {
                    Attributes = Attributes,
                };

                if (Attributes.TryFindAttribute<DefaultSerializerAttribute>(out var _))
                {
                    var drw = ProcessDefaultSerializedMember(member, extra);

                    Members.Add(drw);

                    continue;
                }

                var drawer = member switch
                {
                    MethodInfo I => ProcessMethodInfo(I, extra),
                    PropertyInfo I => ProcessPropertyInfo(I, extra),
                    FieldInfo I => ProcessFieldInfo(I, extra),
                    MemberInfo I => FallbackProcess(I, extra),
                };

                if (drawer is null)
                {
                    continue;
                }

                Members.Add(drawer);
            }
        }

        protected virtual IUiToolkitBase ProcessMethodInfo(MethodInfo MethodInfo, InspectableMemberExtras InspectableMemberExtras)
        {
            if (!InspectableMemberExtras.Attributes.TryFindAttribute<ButtonAttribute>(out var _)) return null;

            var res = new ButtonUiToolkitElement(MethodInfo, InspectableMemberExtras);

            return res;
        }

        protected virtual IUiToolkitBase ProcessPropertyInfo(PropertyInfo PropertyInfo, InspectableMemberExtras InspectableMemberExtras)
        {
            var BackingField = PropertyInfo.GetBackingField();

            if (BackingField is null) return null;

            // if (KnownMappings.TryGetValue(BackingField, out var extras))
            // {
            //     extras.Merge(InspectableMemberExtras);

            //     return null;
            // }

            // KnownMappings[BackingField] = InspectableMemberExtras;

            var res = new UiToolkitPropertyMember(PropertyInfo, BackingField, InspectableMemberExtras);

            return res;
        }
        protected virtual IUiToolkitBase ProcessFieldInfo(FieldInfo FieldInfo, InspectableMemberExtras InspectableMemberExtras)
        {
            if (FieldInfo.IsBackingFieldOfAnAutoProperty())
            {
                return null;
            }

            var res = new UiToolkitFieldMember(FieldInfo, InspectableMemberExtras);

            return res;
        }

        protected virtual IUiToolkitBase ProcessDefaultSerializedMember(MemberInfo MemberInfo, InspectableMemberExtras InspectableMemberExtras)
        {
            var res = new DefaultSerializedUiToolkitElement(MemberInfo, InspectableMemberExtras);

            return res;
        }

        protected virtual IUiToolkitBase FallbackProcess(MemberInfo MemberInfo, InspectableMemberExtras InspectableMemberExtras)
        {
            return null;
        }
    }
}