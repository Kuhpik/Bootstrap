// Made by https://github.com/Razrob
// 2022

using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;
using Sirenix.OdinSerializer.Editor;
using System.IO;
using System.Reflection;
using System.Linq;
using Kuhpik;

public class OdinAOTBuilder : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    private const string assemblyName = "SerializedTypesAssembly";

    public void OnPreprocessBuild(BuildReport report)
    {
        GenerateSerializedTypesDLL();
    }

    [MenuItem("Tools/Odin Serializer/Generate Types DLL")]
    public static void GenerateSerializedTypesDLL()
    {
        List<Type> serializedTypes = new List<Type>();

        DependentTypesFinder.FindDependentTypes(typeof(PlayerData), ref serializedTypes);
        AOTSupportUtilities.GenerateDLL(Application.dataPath, assemblyName, serializedTypes);
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        RemoveGeneratedDLL();
    }

    [MenuItem("Tools/Odin Serializer/Remove Types DLL")]
    public static void RemoveGeneratedDLL()
    {
        File.Delete($"{Application.dataPath}/link.xml");
        File.Delete($"{Application.dataPath}/{assemblyName}.dll");
    }
}


public static class DependentTypesFinder
{
    private static readonly Type[] _excludedTypes = { typeof(Action), typeof(Action<>), typeof(void*), typeof(Delegate) };

    public static void FindDependentTypes(Type type, ref List<Type> resultTypes)
    {
        if (resultTypes.Contains(type) || _excludedTypes.Contains(type))
            return;

        List<Type> basedTypes = FindBaseTypes(type);

        foreach (Type basedType in basedTypes)
        {
            TryAddType(ref resultTypes, basedType);

            if (basedType.IsGenericType)
            {
                List<Type> genericTypes = basedType.GetGenericArguments().ToList();

                foreach (Type genericType in genericTypes)
                    FindDependentTypes(genericType, ref resultTypes);
            }
        }

        foreach (Type basedType in basedTypes)
        {
            List<Type> internalTypesList = basedType.GetFields(
                BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.DeclaredOnly).Select(field => field.FieldType).ToList();

            foreach (Type internalType in internalTypesList)
                FindDependentTypes(internalType, ref resultTypes);
        }
    }

    private static void TryAddType(ref List<Type> types, Type type)
    {
        if (!types.Contains(type) && !_excludedTypes.Contains(type))
            types.Add(type);
    }

    private static List<Type> FindBaseTypes(Type type)
    {
        List<Type> resultTypes = new List<Type>();

        Type currentType = type;

        while (currentType != null && currentType != typeof(ValueType) && currentType != typeof(object))
        {
            resultTypes.Add(currentType);
            currentType = currentType.BaseType;
        }

        return resultTypes;
    }
}
