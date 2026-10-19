using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnnamedUiToolkit
{
    public class ButtonUiToolkitElement : UiToolkitElement, IUiToolkitDraw
    {
        private readonly DynamicCall<Action<object>> Delegate;
        public ButtonUiToolkitElement(MethodInfo MethodInfo, InspectableMemberExtras InspectableMemberExtras)
            : base(InspectableMemberExtras)
        {
            if (MethodInfo is null) return;
            
            Delegate = new()
            {
                Expresion = MethodInfo.CreateUniversalDelegate<Action<object>>(isWeakBind: true),
            };

            Delegate.Compile();
        }

        public DrawerVisitor Draw(DrawerVisitor DrawerVisitor)
        {
            if (!GUILayout.Button(DisplayName)) return DrawerVisitor;

            Undo.RecordObject(DrawerVisitor.RootTarget, $"Executed {DisplayName}");

            Delegate.Compiled(DrawerVisitor.Instance);

            var updateCallbackData = DrawerVisitor.CloneTo<UpdateVisitor>();

            DrawerVisitor.UiEventHandler(updateCallbackData);

            // Should be set dirty bu the update itself (?)
            //EditorUtility.SetDirty(DrawerVisitor.RootTarget);

            return DrawerVisitor;
        }
    }
}