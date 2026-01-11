using System.Reflection;
using System.Text;

namespace Porticle.Reflection.Extensions;

/// <summary>
/// Provides extension methods for converting .NET reflection types into human-readable type strings.
/// </summary>
/// <remarks>
/// This class handles complex type scenarios including nullable reference types, nullable value types,
/// generic types, arrays (jagged and multi-dimensional), and C# type aliases.
/// </remarks>
public static class ReflectionExtensions
{
    /// <summary>
    /// Thread-local instance of NullabilityInfoContext for performance optimization.
    /// Avoids creating a new context for each method call.
    /// </summary>
    private static readonly ThreadLocal<NullabilityInfoContext> NullabilityContext = new(() => new NullabilityInfoContext());

    /// <summary>
    /// Lookup table for C# type aliases to improve performance over repeated if-statements.
    /// </summary>
    private static readonly Dictionary<Type, string> TypeAliases = new()
    {
        { typeof(string), "string" },
        { typeof(sbyte), "sbyte" },
        { typeof(byte), "byte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(nint), "nint" },
        { typeof(nuint), "nuint" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(decimal), "decimal" },
        { typeof(bool), "bool" },
        { typeof(char), "char" },
        { typeof(object), "object" }
    };

    /// <summary>
    /// Converts a <see cref="PropertyInfo"/> to a human-readable type string with nullability information.
    /// </summary>
    /// <param name="property">The property to convert. Cannot be null.</param>
    /// <param name="useFullNames">If true, uses full namespace-qualified type names (e.g., System.String). Default is false.</param>
    /// <param name="useInternalTypeNames">If true, uses C# type aliases (e.g., int instead of Int32). Default is true.</param>
    /// <returns>A readable type string with proper nullability annotations (e.g., "string?", "List&lt;int&gt;").</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="property"/> is null.</exception>
    /// <example>
    /// <code>
    /// PropertyInfo prop = typeof(MyClass).GetProperty("Name");
    /// string typeString = prop.ToReadableTypeString(); // Returns "string?" if nullable
    /// </code>
    /// </example>
    public static string ToReadableTypeString(this PropertyInfo property, bool useFullNames = false, bool useInternalTypeNames = true)
    {
        ArgumentNullException.ThrowIfNull(property);
        var nullabilityInfo = NullabilityContext.Value!.Create(property);
        return ToReadableTypeStringInternal(property.PropertyType, nullabilityInfo, useFullNames, useInternalTypeNames);
    }

    /// <summary>
    /// Converts a <see cref="ParameterInfo"/> to a human-readable type string with nullability information.
    /// </summary>
    /// <param name="parameter">The parameter to convert. Cannot be null.</param>
    /// <param name="useFullNames">If true, uses full namespace-qualified type names. Default is false.</param>
    /// <param name="useInternalTypeNames">If true, uses C# type aliases. Default is true.</param>
    /// <returns>A readable type string with proper nullability annotations.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
    /// <example>
    /// <code>
    /// MethodInfo method = typeof(MyClass).GetMethod("DoSomething");
    /// ParameterInfo param = method.GetParameters()[0];
    /// string typeString = param.ToReadableTypeString(); // Returns "List&lt;string?&gt;?" if nullable
    /// </code>
    /// </example>
    public static string ToReadableTypeString(this ParameterInfo parameter, bool useFullNames = false, bool useInternalTypeNames = true)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        var nullabilityInfo = NullabilityContext.Value!.Create(parameter);
        return ToReadableTypeStringInternal(parameter.ParameterType, nullabilityInfo, useFullNames, useInternalTypeNames);
    }

    /// <summary>
    /// Converts a <see cref="FieldInfo"/> to a human-readable type string with nullability information.
    /// </summary>
    /// <param name="field">The field to convert. Cannot be null.</param>
    /// <param name="useFullNames">If true, uses full namespace-qualified type names. Default is false.</param>
    /// <param name="useInternalTypeNames">If true, uses C# type aliases. Default is true.</param>
    /// <returns>A readable type string with proper nullability annotations.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="field"/> is null.</exception>
    /// <example>
    /// <code>
    /// FieldInfo field = typeof(MyClass).GetField("_name", BindingFlags.NonPublic | BindingFlags.Instance);
    /// string typeString = field.ToReadableTypeString(); // Returns "string?" if nullable
    /// </code>
    /// </example>
    public static string ToReadableTypeString(this FieldInfo field, bool useFullNames = false, bool useInternalTypeNames = true)
    {
        ArgumentNullException.ThrowIfNull(field);
        var nullabilityInfo = NullabilityContext.Value!.Create(field);
        return ToReadableTypeStringInternal(field.FieldType, nullabilityInfo, useFullNames, useInternalTypeNames);
    }

    /// <summary>
    /// Converts an <see cref="EventInfo"/> to a human-readable type string with nullability information.
    /// </summary>
    /// <param name="eventInfo">The event to convert. Cannot be null and must have a valid EventHandlerType.</param>
    /// <param name="useFullNames">If true, uses full namespace-qualified type names. Default is false.</param>
    /// <param name="useInternalTypeNames">If true, uses C# type aliases. Default is true.</param>
    /// <returns>A readable type string representing the event handler type (e.g., "EventHandler?", "Action&lt;string?&gt;").</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="eventInfo"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when EventHandlerType is null.</exception>
    /// <example>
    /// <code>
    /// EventInfo evt = typeof(MyClass).GetEvent("PropertyChanged");
    /// string typeString = evt.ToReadableTypeString(); // Returns "PropertyChangedEventHandler?"
    /// </code>
    /// </example>
    public static string ToReadableTypeString(this EventInfo eventInfo, bool useFullNames = false, bool useInternalTypeNames = true)
    {
        ArgumentNullException.ThrowIfNull(eventInfo);

        if (eventInfo.EventHandlerType == null)
        {
            throw new ArgumentException("EventInfo must have a valid EventHandlerType", nameof(eventInfo));
        }

        var nullabilityInfo = NullabilityContext.Value!.Create(eventInfo);
        return ToReadableTypeStringInternal(eventInfo.EventHandlerType, nullabilityInfo, useFullNames, useInternalTypeNames);
    }

    /// <summary>
    /// Converts a <see cref="Type"/> to a human-readable type string with optional nullability information.
    /// </summary>
    /// <param name="type">The type to convert. Cannot be null.</param>
    /// <param name="nullabilityInfo">Optional nullability information. If null, nullability annotations will not be included.</param>
    /// <param name="useFullnames">If true, uses full namespace-qualified type names. Default is false.</param>
    /// <param name="useInternalTypeNames">If true, uses C# type aliases. Default is true.</param>
    /// <returns>A readable type string (e.g., "List&lt;int&gt;", "Dictionary&lt;string,int&gt;").</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is null.</exception>
    /// <remarks>
    /// Note: Due to type erasure, nullable reference types like List&lt;string?&gt; cannot preserve nullability
    /// when using typeof(). Use PropertyInfo, ParameterInfo, FieldInfo, or EventInfo overloads for accurate
    /// nullable reference type detection. Nullable value types (e.g., int?) work correctly with typeof().
    /// </remarks>
    /// <example>
    /// <code>
    /// Type type = typeof(Dictionary&lt;string, List&lt;int&gt;&gt;);
    /// string typeString = type.ToReadableTypeString(); // Returns "Dictionary&lt;string,List&lt;int&gt;&gt;"
    /// </code>
    /// </example>
    public static string ToReadableTypeString(this Type type, NullabilityInfo? nullabilityInfo = null, bool useFullnames = false, bool useInternalTypeNames = true)
    {
        ArgumentNullException.ThrowIfNull(type);
        return ToReadableTypeStringInternal(type, nullabilityInfo, useFullnames, useInternalTypeNames);
    }

    /// <summary>
    /// Internal recursive implementation that handles the actual type-to-string conversion logic.
    /// </summary>
    /// <param name="type">The type to convert.</param>
    /// <param name="nullabilityInfo">Nullability information for the type.</param>
    /// <param name="useFullNames">Whether to use full namespace-qualified names.</param>
    /// <param name="useInternalTypeNames">Whether to use C# type aliases.</param>
    /// <returns>A readable type string.</returns>
    /// <remarks>
    /// Processing order:
    /// 1. Extract nullable value types (Nullable&lt;T&gt;) and determine nullability suffix
    /// 2. Check for C# type aliases (int, string, bool, etc.)
    /// 3. Handle arrays (jagged [][], multi-dimensional [,])
    /// 4. Handle generic types with recursive processing
    /// 5. Return simple type names with optional namespace
    /// </remarks>
    private static string ToReadableTypeStringInternal(Type type, NullabilityInfo? nullabilityInfo, bool useFullNames, bool useInternalTypeNames)
    {
        ArgumentNullException.ThrowIfNull(type);

        // Step 1: Check for nullable value types (e.g., int?, DateTime?)
        // Nullable<T> is unwrapped to its underlying type with a "?" suffix
        var underlyingType = Nullable.GetUnderlyingType(type);
        string suffix;

        if (underlyingType != null)
        {
            // This is a nullable value type (Nullable<T>)
            type = underlyingType;
            suffix = "?";
        }
        else
        {
            // Check for nullable reference types using NullabilityInfo
            suffix = nullabilityInfo?.ReadState == NullabilityState.Nullable ? "?" : "";
        }

        // Step 2: Check for C# type aliases (int, string, bool, etc.)
        // Only applied if useInternalTypeNames is true
        if (useInternalTypeNames && TypeAliases.TryGetValue(type, out var alias))
        {
            return alias + suffix;
        }

        // Step 3: Handle array types
        // Distinguishes between jagged arrays ([][]) and multi-dimensional arrays ([,])
        if (type.IsArray)
        {
            var elementType = type.GetElementType()!;
            var elementTypeString = ToReadableTypeStringInternal(elementType, nullabilityInfo?.ElementType, useFullNames, useInternalTypeNames);
            var sb = new StringBuilder(elementTypeString);

            if (type.IsVariableBoundArray)
            {
                // Multi-dimensional array (e.g., [,] or [,,])
                sb.Append('[');
                sb.Append(new string(',', type.GetArrayRank() - 1));
                sb.Append(']');
            }
            else
            {
                // Jagged array (e.g., [][] or [][][])
                for (var i = 0; i < type.GetArrayRank(); i++)
                {
                    sb.Append("[]");
                }
            }

            sb.Append(suffix);
            return sb.ToString();
        }

        // Step 4: Handle generic types (e.g., List<T>, Dictionary<K,V>)
        // Recursively processes generic type arguments
        if (type.IsGenericType)
        {
            var typeName = useFullNames ? (type.FullName ?? type.Name) : type.Name;
            var baseTypeName = typeName.Split("`").First();

            var genericArgs = type.GenericTypeArguments;
            var genericArgStrings = new List<string>(genericArgs.Length);

            for (int i = 0; i < genericArgs.Length; i++)
            {
                var argNullability = nullabilityInfo?.GenericTypeArguments.ElementAtOrDefault(i);
                var argString = ToReadableTypeStringInternal(genericArgs[i], argNullability, useFullNames, useInternalTypeNames);
                genericArgStrings.Add(argString);
            }

            return $"{baseTypeName}<{string.Join(",", genericArgStrings)}>{suffix}";
        }

        // Step 5: Return simple type name (with optional full namespace)
        var finalName = useFullNames ? (type.FullName ?? type.Name) : type.Name;
        return finalName + suffix;
    }
}