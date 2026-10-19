// using System.Reflection;
// using UnityEditor;

// public abstract class InspectableCustomValueUndoMember : InspectableCustomValueMember
// {
//     protected InspectableCustomValueUndoMember(MemberInfo MemberInfo, InspectableMemberExtras InspectableMemberExtras)
//         : base(MemberInfo, InspectableMemberExtras) { }

//     public override object OnValueBeforeDraw(object value)
//     {
//         if (GetPropertyIfSerializable() is SerializedProperty sp)
//         {
//             var serializedValue = sp.boxedValue;

//             return serializedValue;
//         }

//         return value;
//     }

//     public override void OnValueAfterChange(object instance, object value)
//     {
//         var serializationTarget = SerializedGroup.SerializationTarget;

//         base.OnValueAfterChange(instance, value);

//         if (GetPropertyIfSerializable() is SerializedProperty sp)
//         {
           
//         }
        
//     }

//     protected SerializedProperty SerializedPropertyCache;
   
// }