using System.Reflection;
using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Utility;

public static class TypeUtility
{
    public static Type[] GetTypesWithAttribute(Type pAttribute)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        List<Type> types = new List<Type>();
        foreach (Assembly assembly in assemblies)
        {
            types.AddRange(assembly.GetTypes().Where(e => e.IsDefined(typeof(SettingsAttribute))));
        }

        return types.ToArray();
    }

    /// <summary>
    /// Get all types that inherit from a provided parent type that are not by itself an interface or abstract
    /// </summary>
    public static Type[] GetTypesBasedOnAbstractParent(Type pAbstractParent)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        List<Type> types = new List<Type>();
        foreach (Assembly assembly in assemblies)
        {
            types.AddRange(assembly.GetTypes().Where(
                e => e.IsAssignableFrom(pAbstractParent) && e is {IsAbstract: false, IsInterface: true}));
        }

        return types.ToArray();
    }
}
