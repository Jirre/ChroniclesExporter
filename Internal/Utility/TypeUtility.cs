using System.Reflection;

namespace ChroniclesExporter.Utility;

public static class TypeUtility
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

        List<Type> types = new();
        foreach (Assembly assembly in assemblies)
            types.AddRange(assembly.GetTypes().Where(
                pType =>
                    pType.IsClass &&
                    pType is {IsAbstract: false, IsInterface: false} &&
                    pAbstractParent.IsAssignableFrom(pType)));

        return types.ToArray();
    }
}
