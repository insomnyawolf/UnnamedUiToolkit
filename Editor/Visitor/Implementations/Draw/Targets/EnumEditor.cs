using System;
using System.Linq.Expressions;
using System.Reflection;

namespace UnnamedUiToolkit
{
    public class EnumEditor : DrawerVisitorTarget
    {
        protected static InspectorBinding BaseCache = InspectorBindingCache.EnumDrawer;
        public EnumEditor(IUiToolkitValueMember Member)
            : base(Member) { }

        public override NodeVisitor RunNode(NodeVisitor Visitor)
        {
            var values = Visitor.Values;

            var oldValEnum = Enum.ToObject(Member.MemberType, values.Value);

            // I hate this and i don't know what else to do
            // Workaround to prevent saving when value doesn't change
            // Tryed applying it on the get but if it's serialized it fails bc unity returns a number
            // This is the cleanest place since this only affects enums
            values.Update(oldValEnum);
            
            var newVal = BaseCache.Compiled(Member.DisplayName, oldValEnum, Constants.EmptyGuiLayoutOptions);

            values.Update(newVal);

            return Visitor;
        }

        // public static DynamicCall<GetDelegate> AddEnumToObjectOnReturnConversion(DynamicCall<GetDelegate> Get, Type enumType)
        // {
        //     var Expr = Get.Expresion;

        //     var Body = Expr.Body;

        //     var enumTypeE = Expression.Constant(enumType, typeof(Type));

        //     var callExpression = Expression.Call(null, EnumToObjectCache, enumTypeE, Body);

        //     var newExpr = Expr.Update(callExpression, Expr.Parameters);

        //     Get.Expresion = newExpr;

        //     Get.Compile();

        //     // Unneded but explicit
        //     return Get;
        // }

        // private static readonly MethodInfo EnumToObjectCache = typeof(Enum)
        // .GetMethod(
        //     nameof(Enum.ToObject), 
        //     new Type[] { 
        //         typeof(Type), 
        //         typeof(object) 
        //     }
        // );
    }
}