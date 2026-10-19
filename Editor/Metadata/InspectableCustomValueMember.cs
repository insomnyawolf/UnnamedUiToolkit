// using System;
// using System.Reflection;
// using UnityEditor;

// public abstract class InspectableCustomValueMember : InspectableValueMember
// {
//     public bool IsReadonly { get; private set; }
//     public bool IsRuntimeOnly { get; private set; }
//     public abstract Type MemberType { get; }
//     public OnChange OnChange { get; protected set; }
//     protected InspectableCustomValueMember(MemberInfo MemberInfo, InspectableMemberExtras InspectableMemberExtras)
//         : base(MemberInfo, InspectableMemberExtras) { }

//     protected override void ProcessAttribute(Attribute Attribute)
//     {
//         if (Attribute is ReadOnlyAttribute _)
//         {
//             IsReadonly = true;
//             return;
//         }

//         if (Attribute is RuntimeOnlyAttribute _)
//         {
//             IsRuntimeOnly = true;
//             return;
//         }

//         if (Attribute is OnChangeAttribute OnChange)
//         {
//             var expr = OnChange.ProcessOnChangeAttribute(MemberInfo);

//             this.OnChange = new()
//             {
//                 Expresion = expr,
//                 Compiled = expr.Compile(),
//             };

//             return;
//         }

//         base.ProcessAttribute(Attribute);
//     }

//     protected override void Visit(ref VisitData VisitData)
//     {
//         DrawInternal(ref VisitData);
//         return;
//     }

//     protected virtual void DrawInternal(ref VisitData VisitData)
//     {
//         using var Readonly = new EditorGUI.DisabledGroupScope(disabled: IsReadonly);

//         DrawWrapper(ref VisitData);
//     }

//     protected void DrawWrapper(ref VisitData VisitData)
//     {
//         var instance = VisitData.Target;

//         var currentValue = SafeCall(Getter, instance);

//         currentValue = OnValueBeforeDraw(currentValue);

//         VisitData.Target = currentValue;
//         VisitData.Instance = instance;

//         EditorGUI.BeginChangeCheck();

//         var newValue = Draw(ref VisitData);

//         // Is this if needed?
//         if (newValue == currentValue)
//         {
//             return;
//         }

//         if (!EditorGUI.EndChangeCheck())
//         {
//             return;
//         }
        
//         OnValueAfterChange(instance, newValue);
//     }

//     protected object Draw(ref VisitData VisitData)
//     {
//         var newValue = Cache.Draw(ref VisitData);

//         return newValue;
//     }

//     public virtual object OnValueBeforeDraw(object value) => value;
//     public virtual void OnValueAfterChange(object instance, object value)
//     {
//         SafeCall(Setter, instance, value);
//     }

//     protected void FinishInitialization()
//     {
//         Getter.Compile();

//         Setter.Compile();

//         Setter.OnChange = OnChange;

//         CreateCachedDrawer();
//     }

//     protected BaseEditor Cache;
//     protected void CreateCachedDrawer()
//     {
//         if (EnumEditor.IsTarget(MemberType))
//         {
//             Cache = new EnumEditor(this);
//             return;
//         }

//         if (ArrayEditor.IsTarget(MemberType))
//         {
//             Cache = new ArrayEditor(this);
//             return;
//         }

//         if (PrimitiveEditor.IsTarget(MemberType, out var binding))
//         {
//             Cache = new PrimitiveEditor(this, binding);
//             return;
//         }

//         if (ChildEditor.IsTarget(MemberType))
//         {
//             Cache = new ChildEditor(this);
//             return;
//         }

//         if (NativeUnityObjectEditor.IsTarget(MemberType))
//         {
//             Cache = new NativeUnityObjectEditor(this);
//             return;
//         }

//         throw new Exception("This Should Not Happen");
//     }
// }