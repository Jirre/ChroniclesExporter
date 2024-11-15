using System.Reflection;

namespace ChroniclesExporter.Utility;

public static class ReflectionUtility
{
    public static Type[] GetTypesWithAttribute(Type pAttribute)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        List<Type> types = new();
        foreach (Assembly assembly in assemblies)
            types.AddRange(assembly.GetTypes().Where(pType => pType.IsDefined(pAttribute)));

        return types.ToArray();
    }

    /// <summary>
    ///     Get all types that inherit from a provided parent type that are not by itself an interface or abstract
    /// </summary>
    public static Type[] GetTypesBasedOnAbstractParent(Type pAbstractParent)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        List<Type> types = [];
        foreach (Assembly assembly in assemblies)
        {
            types.AddRange(assembly.GetTypes().Where(
                pType =>
                    pType.IsClass &&
                    pType is {IsAbstract: false, IsInterface: false} &&
                    pAbstractParent.IsAssignableFrom(pType)));
        }

        return types.ToArray();
    }
    
    public static MethodInfo[] GetMethodsWithAttribute(Type pAttribute, bool pInherit = false)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        List<MethodInfo> methods = [];
        foreach (Assembly assembly in assemblies)
        {
            foreach (Type type in assembly.GetTypes())
            {
                methods.AddRange(type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(pMethod => pMethod.IsDefined(pAttribute, inherit: pInherit)));
            }
        }
        return methods.ToArray();
    }
}
