using System;
using System.Collections.Generic;
using UnityEditor;

namespace UnnamedUiToolkit
{
    public abstract class UiToolkitElement : IUiToolkitElement
    {
        public InspectableMemberExtras InspectableMemberExtras { get; protected set; }
        public List<Attribute> Attributes => InspectableMemberExtras.Attributes;
        public string DisplayName { get; protected set; }
        public bool IsSkip { get; protected set; }
        public bool IsHide { get; protected set; }

        protected UiToolkitElement(InspectableMemberExtras InspectableMemberExtras)
        {
            this.InspectableMemberExtras = InspectableMemberExtras ?? new InspectableMemberExtras();

            foreach (var attr in Attributes)
            {
                ProcessAttribute(attr);
            }
        }

        protected virtual void ProcessAttribute(Attribute Attribute)
        {
            if (Attribute is DisplayNameAttribute DisplayName)
            {
                this.DisplayName = ObjectNames.NicifyVariableName(DisplayName.Name);
                return;
            }

            if (Attribute is SkipAttribute _)
            {
                IsSkip = true;
                return;
            }

            if (Attribute is HideAttribute _)
            {
                IsHide = true;
                return;
            }
        }
    }
}