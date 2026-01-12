using System.Reflection;
using Porticle.Reflection.Extensions;

namespace Porticle.Reflection.Extensions.Tests;

/// <summary>
/// Common test fixtures and helper classes used across multiple test files.
/// </summary>
public static class TestFixtures
{
    /// <summary>
    /// Test class with various nullable property configurations.
    /// </summary>
    public class NullablePropertiesClass
    {
        public string NonNullableString { get; set; } = "";
        public string? NullableString { get; set; }
        public int NonNullableInt { get; set; }
        public int? NullableInt { get; set; }
        public List<string> NonNullableList { get; set; } = new();
        public List<string>? NullableList { get; set; }
        public List<int?> ListOfNullableInt { get; set; } = new();
        public List<string?> ListOfNullableString { get; set; } = new();
        public List<int?>? NullableListOfNullableInt { get; set; }
        public List<string?>? NullableListOfNullableString { get; set; }
    }

    /// <summary>
    /// Test class with various nullable field configurations.
    /// </summary>
    public class NullableFieldsClass
    {
        public List<string> NonNullableList = new();
        public List<string>? NullableList;
        public List<int?> ListOfNullableInt = new();
        public List<string?> ListOfNullableString = new();
        public List<string?>? NullableListOfNullableString;
    }

    /// <summary>
    /// Test class with method parameters for testing ParameterInfo.
    /// </summary>
    public class MethodParametersClass
    {
        public void MethodWithParameters(
            List<string> nonNullableList,
            List<string>? nullableList,
            List<int?> listOfNullableInt,
            List<string?> listOfNullableString,
            List<string?>? nullableListOfNullableString)
        {
        }
    }

    /// <summary>
    /// Custom event handler delegates for testing.
    /// </summary>
    public delegate void SimpleEventHandler();
    public delegate void StringEventHandler(string message);
    public delegate void GenericEventHandler<T>(T data);

    /// <summary>
    /// Test class with various event configurations.
    /// </summary>
#pragma warning disable CS0067 // Event is never used - events are accessed via reflection in tests
    public class EventsClass
    {
        // Standard EventHandler
        public event EventHandler? StandardEvent;

        // Custom delegate events
        public event SimpleEventHandler? SimpleEvent;
        public event StringEventHandler? StringEvent;

        // Generic event handlers
        public event EventHandler<string>? GenericStringEvent;
        public event EventHandler<int>? GenericIntEvent;
        public event EventHandler<List<string>>? GenericListEvent;
        public event EventHandler<List<string?>>? GenericListOfNullableStringEvent;
        public event EventHandler<List<int?>>? GenericListOfNullableIntEvent;

        // Non-nullable event (rare, but possible)
        public event EventHandler NonNullableEvent = delegate { };

        // Action and Func events
        public event Action? ActionEvent;
        public event Action<string>? ActionStringEvent;
        public event Action<string?>? ActionNullableStringEvent;
        public event Func<int>? FuncIntEvent;
        public event Func<string?>? FuncNullableStringEvent;
    }
#pragma warning restore CS0067

    /// <summary>
    /// Helper methods to reduce test code duplication.
    /// </summary>
    public static class TestHelpers
    {
        /// <summary>
        /// Gets a property from a type by name.
        /// </summary>
        public static PropertyInfo GetProperty<T>(string propertyName)
        {
            return typeof(T).GetProperty(propertyName)
                ?? throw new InvalidOperationException($"Property '{propertyName}' not found on type '{typeof(T).Name}'");
        }

        /// <summary>
        /// Gets a field from a type by name.
        /// </summary>
        public static FieldInfo GetField<T>(string fieldName)
        {
            return typeof(T).GetField(fieldName)
                ?? throw new InvalidOperationException($"Field '{fieldName}' not found on type '{typeof(T).Name}'");
        }

        /// <summary>
        /// Gets a parameter from a method by method and parameter index.
        /// </summary>
        public static ParameterInfo GetParameter<T>(string methodName, int parameterIndex)
        {
            var method = typeof(T).GetMethod(methodName)
                ?? throw new InvalidOperationException($"Method '{methodName}' not found on type '{typeof(T).Name}'");

            var parameters = method.GetParameters();
            if (parameterIndex < 0 || parameterIndex >= parameters.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(parameterIndex),
                    $"Parameter index {parameterIndex} is out of range. Method has {parameters.Length} parameters.");
            }

            return parameters[parameterIndex];
        }

        /// <summary>
        /// Gets an event from a type by name.
        /// </summary>
        public static EventInfo GetEvent<T>(string eventName)
        {
            return typeof(T).GetEvent(eventName)
                ?? throw new InvalidOperationException($"Event '{eventName}' not found on type '{typeof(T).Name}'");
        }
    }
}
