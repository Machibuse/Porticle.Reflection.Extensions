# Porticle.Reflection.Extensions

[![NuGet Version](https://img.shields.io/nuget/v/Porticle.Reflection.Extensions)](https://www.nuget.org/packages/Porticle.Reflection.Extensions)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Porticle.Reflection.Extensions)](https://www.nuget.org/packages/Porticle.Reflection.Extensions)
[![Build](https://github.com/Machibuse/Porticle.Reflection.Extensions/actions/workflows/release.yaml/badge.svg)](https://github.com/Machibuse/Porticle.Reflection.Extensions/actions)
[![License](https://img.shields.io/github/license/Machibuse/Porticle.Reflection.Extensions)](https://github.com/Machibuse/Porticle.Reflection.Extensions/blob/main/LICENSE)

## Overview
A .NET library that provides extension methods for converting .NET reflection types into human-readable type strings. This library handles complex scenarios including nullable reference types, nullable value types, generic types, arrays, and C# type aliases, making reflection output easier to read and understand.

For example, when you have a Propterty of type like `IDictionary<MyClass?[][], Dictionary<object?,int?[,]?>>?`, the library is able to convert it to a readable c# type name that looks mostly exact like the normal c# type name in most cases.

When you use .ToString() you will get 
```csharp
System.Collections.Generic.IDictionary`2[MyClass[][],System.Collections.Generic.Dictionary`2[System.Object,System.Nullable`1[System.Int32][,]]]
```

When you use this library you will get
```csharp
IDictionary<MyClass?[][], Dictionary<object?,int?[,]?>>?
```

## Key Features
The library automatically converts reflection types to readable strings:
- Nullable reference types (`string?`, `List<string>?`)
- Nullable value types (`int?`, `DateTime?`)
- Generic types with proper formatting (`List<T>`, `Dictionary<TKey, TValue>`)
- Arrays (single-dimensional `[]`, jagged `[][]`, and multi-dimensional `[,]`)
- C# type aliases (`int`, `string`, `bool` instead of `Int32`, `String`, `Boolean`)
- Event handlers (`EventHandler<T>`, `Action<T>`, `Func<T>`)
- Nested generic types (`List<List<string?>>?`)

## Installation
Install via NuGet:
```bash
dotnet add package Porticle.Reflection.Extensions
```

Or via Package Manager Console:
```powershell
Install-Package Porticle.Reflection.Extensions
```

## Usage

### Basic Type Conversion
```csharp
using Porticle.Reflection.Extensions;

// Simple types
Type intType = typeof(int);
Console.WriteLine(intType.ToReadableTypeString()); // Output: int

Type stringType = typeof(string);
Console.WriteLine(stringType.ToReadableTypeString()); // Output: string

// Nullable value types
Type nullableInt = typeof(int?);
Console.WriteLine(nullableInt.ToReadableTypeString()); // Output: int?

// Generic types
Type listType = typeof(List<string>);
Console.WriteLine(listType.ToReadableTypeString()); // Output: List<string>

Type dictType = typeof(Dictionary<int, string>);
Console.WriteLine(dictType.ToReadableTypeString()); // Output: Dictionary<int, string>
```

### Working with Properties (Nullable Reference Types)
```csharp
public class User
{
    public string Name { get; set; } = "";
    public string? Email { get; set; }
    public List<string>? Tags { get; set; }
    public List<string?> Nicknames { get; set; } = new();
}

// Using PropertyInfo to get nullable reference type information
PropertyInfo nameProperty = typeof(User).GetProperty("Name")!;
Console.WriteLine(nameProperty.ToReadableTypeString()); // Output: string

PropertyInfo emailProperty = typeof(User).GetProperty("Email")!;
Console.WriteLine(emailProperty.ToReadableTypeString()); // Output: string?

PropertyInfo tagsProperty = typeof(User).GetProperty("Tags")!;
Console.WriteLine(tagsProperty.ToReadableTypeString()); // Output: List<string>?

PropertyInfo nicknamesProperty = typeof(User).GetProperty("Nicknames")!;
Console.WriteLine(nicknamesProperty.ToReadableTypeString()); // Output: List<string?>
```

### Working with Method Parameters
```csharp
public class DataService
{
    public void ProcessData(
        List<string> requiredItems,
        List<string>? optionalItems,
        List<string?> itemsWithNullableElements)
    {
    }
}

MethodInfo method = typeof(DataService).GetMethod("ProcessData")!;
ParameterInfo[] parameters = method.GetParameters();

Console.WriteLine(parameters[0].ToReadableTypeString()); // Output: List<string>
Console.WriteLine(parameters[1].ToReadableTypeString()); // Output: List<string>?
Console.WriteLine(parameters[2].ToReadableTypeString()); // Output: List<string?>
```

### Working with Fields and Events
```csharp
public class EventDemo
{
    public List<string>? Items;
    public event EventHandler<string>? DataReceived;
}

// Fields
FieldInfo field = typeof(EventDemo).GetField("Items")!;
Console.WriteLine(field.ToReadableTypeString()); // Output: List<string>?

// Events
EventInfo evt = typeof(EventDemo).GetEvent("DataReceived")!;
Console.WriteLine(evt.ToReadableTypeString()); // Output: EventHandler<string>?
```

## Advanced Features

### Full Type Names
Use `useFullNames: true` to include full namespaces:
```csharp
Type listType = typeof(List<string>);
Console.WriteLine(listType.ToReadableTypeString(useFullNames: true));
// Output: System.Collections.Generic.List<System.String>
```

### Native .NET Type Names
Use `useInternalTypeNames: false` to get .NET type names instead of C# aliases:
```csharp
Type intType = typeof(int);
Console.WriteLine(intType.ToReadableTypeString(useInternalTypeNames: false));
// Output: Int32

Type stringType = typeof(string);
Console.WriteLine(stringType.ToReadableTypeString(useInternalTypeNames: false));
// Output: String
```

### Arrays
```csharp
// Single-dimensional array
Type intArray = typeof(int[]);
Console.WriteLine(intArray.ToReadableTypeString()); // Output: int[]

// Jagged array
Type jaggedArray = typeof(int[][]);
Console.WriteLine(jaggedArray.ToReadableTypeString()); // Output: int[][]

// Multi-dimensional array
Type multiArray = typeof(int[,]);
Console.WriteLine(multiArray.ToReadableTypeString()); // Output: int[,]
```

## Limitations

### Type Erasure
Due to .NET's type erasure for nullable reference types, `typeof()` cannot preserve nullability information for generic type arguments:

```csharp
// ❌ This will NOT show the nullable string
Type listType = typeof(List<string?>);
Console.WriteLine(listType.ToReadableTypeString()); // Output: List<string> (nullability lost)

// ✅ Use PropertyInfo, ParameterInfo, FieldInfo, or EventInfo instead
public class Example
{
    public List<string?> Items { get; set; } = new();
}

PropertyInfo property = typeof(Example).GetProperty("Items")!;
Console.WriteLine(property.ToReadableTypeString()); // Output: List<string?> (correct)
```

Note: Nullable **value types** (like `List<int?>`) work correctly with `typeof()` because `Nullable<T>` is a real runtime type.

## Target Frameworks
- .NET 10.0
- .NET 9.0
- .NET 8.0

## License
MIT
