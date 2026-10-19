using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace UnnamedUiToolkit
{
    public static class InspectorBindingCache
{
    public static readonly IReadOnlyList<InspectorBinding> EditorDrawerFunctionsCache = GenerateBindingsCache();
    public static readonly IReadOnlyDictionary<Type, InspectorBinding> PropertyDrawersCache = GeneratePropertyDrawersCache();
    public static readonly InspectorBinding EnumDrawer = GetEnumDrawer();

    private static List<InspectorBinding> GenerateBindingsCache()
    {
        var parentClass = typeof(EditorGUILayout);

        var methods = parentClass.GetMethods(BindingFlags.Static | BindingFlags.Public);

        var filteredMethods = methods.Select(item => new MethodInfoParameterCache(item));

        filteredMethods = filteredMethods.Where(item => item.IsStatic);

        filteredMethods = filteredMethods.Where(item => !item.Name.Contains("lambda_method"));

        filteredMethods = filteredMethods.Where(item => !item.IsForSerializedProperty);

        filteredMethods = filteredMethods.Where(item => item.HasCorrectNumberOfParams);

        filteredMethods = filteredMethods.Where(item => item.HasLabel);

        filteredMethods = filteredMethods.Where(item => item.AnyParamMatchReturnType);

        filteredMethods = filteredMethods.Where(item => !item.HasGUIStyle);

        filteredMethods = filteredMethods.Where(item => item.HasGUILayoutOption);

        filteredMethods = filteredMethods.Where(item => !item.Name.Contains("Delayed"));

        //filteredMethods = filteredMethods.Where(item => item.Name.EndsWith("Field"));

        var ready = new List<InspectorBinding>();

        foreach (var item in filteredMethods)
        {
            var bind = item.GetEditorBinding();

            ready.Add(bind);
        }

        //DebugData(ready);

        return ready;
    }

    private static Dictionary<Type, InspectorBinding> GeneratePropertyDrawersCache()
    {
        var cache = new Dictionary<Type, InspectorBinding>();

        foreach (var item in EditorDrawerFunctionsCache)
        {
            var returnType = item.MethodInfo.ReturnType;

            cache.TryAdd(returnType, item);
        }

        return cache;
    }

    private static InspectorBinding GetEnumDrawer()
    {
        foreach (var item in EditorDrawerFunctionsCache)
        {
            if (item.Name == "EnumPopup")
            {
                return item;
            }
        }

        return null;
    }


    private static void DebugData(List<MethodInfoParameterCache> input)
    {
        var debugCache = new Dictionary<string, List<MethodInfoParameterCache>>();

        foreach (var item in input)
        {
            var name = item.Name;

            name = name.Replace("Delayed", "");

            if (!debugCache.TryGetValue(name, out var val))
            {
                val = new();
                debugCache[name] = val;
            }

            val.Add(item);
        }

        var debug = JsonConvert.SerializeObject(debugCache, formatting: Formatting.Indented);

        Debug.LogWarning(debug);

        Console.WriteLine();
    }

    //private static readonly IReadOnlyDictionary<Type, Func<string, dynamic, dynamic>> PropertyDrawersCache = new Dictionary<Type, Func<string, dynamic, dynamic>>()
    //{
    //    {typeof(int), (a, b) => EditorGUILayout.IntField(a, b) },
    //    {typeof(float), (a, b) => EditorGUILayout.FloatField(a, b) },
    //    {typeof(double), (a, b) => EditorGUILayout.DoubleField(a, b) },
    //    {typeof(bool), (a, b) => EditorGUILayout.Toggle(a, b) },
    //    {typeof(string), (a, b) => EditorGUILayout.TextField(a, b) },
    //    {typeof(Vector2), (a, b) => EditorGUILayout.Vector2Field(a, b) },
    //    {typeof(Vector3), (a, b) => EditorGUILayout.Vector3Field(a, b) },
    //    {typeof(Vector4), (a, b) => EditorGUILayout.Vector4Field(a, b) },
    //    {typeof(Color), (a, b) => EditorGUILayout.ColorField(a, b) },
    //    {typeof(AnimationCurve), (a, b) => EditorGUILayout.CurveField(a, b) },
    //};
}

}