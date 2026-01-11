# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ReflectionExtensions is a .NET 10.0 library that provides extension methods for converting .NET reflection types into human-readable type strings. The library handles complex scenarios including:
- Nullable reference types and nullable value types
- Generic types with proper formatting
- Arrays (both standard and variable-bound/multi-dimensional)
- C# type aliases (int, string, bool, etc.)

## Core Architecture

**Single-file library**: The entire implementation is in `ReflectionExtensions/ReflectionExtensions.cs`

**Key Components**:
- `ToReadableTypeString(this PropertyInfo)` - Converts PropertyInfo to readable type string with nullability context
- `ToReadableTypeString(this ParameterInfo)` - Converts ParameterInfo to readable type string with nullability context
- `ToReadableTypeString(this FieldInfo)` - Converts FieldInfo to readable type string with nullability context
- `ToReadableTypeString(this EventInfo)` - Converts EventInfo to readable type string with nullability context
- `ToReadableTypeString(this Type, NullabilityInfo?)` - Converts Type to readable string with optional nullability info
- `ToReadableTypeStringInternal()` - Private recursive implementation handling all type conversion logic

**Type Handling Order**:
1. Nullable types (extracts underlying type, appends `?`)
2. C# type aliases (when `useInternalTypeNames=true`)
3. Arrays (handles both jagged `[][]` and multi-dimensional `[,]` syntax)
4. Generic types (recursive processing with `<>` syntax)
5. Simple types (with optional full namespace via `useFullNames`)

## Build Commands

```bash
# Build the solution
dotnet build

# Build in Release configuration
dotnet build -c Release

# Clean build artifacts
dotnet clean

# Run all tests
dotnet test

# Run tests with detailed output
dotnet test -v detailed

# Run a specific test
dotnet test --filter "FullyQualifiedName~ToReadableTypeString_IntType"
```

## Testing

**Test Project**: `ReflectionExtensions.Tests` using xUnit

**Test Coverage** (85+ tests covering):
- All primitive types and C# type aliases (int, string, bool, etc.)
- Nullable value types (int?, bool?, DateTime?)
- Nullable reference types via PropertyInfo (string?, List<string>?, List<string?>)
- Nullable reference types via ParameterInfo (List<string?>?, etc.)
- Nullable reference types via FieldInfo (List<string?>?, etc.)
- Nullable reference types via EventInfo (EventHandler?, Action<string?>?, etc.)
- Arrays (single-dimensional, jagged, and multi-dimensional)
- Generic types (List<T>, Dictionary<K,V>, nested generics)
- Event handlers (EventHandler, EventHandler<T>, Action, Func, custom delegates)
- Parameter combinations (useFullnames, useInternalTypeNames)
- Edge cases (void, Task<T>, arrays of generics, type erasure)

**Test Files**:
- `ReflectionExtensionsTests.cs` - Main tests for Type and PropertyInfo overloads
- `ParameterAndFieldTests.cs` - Tests for ParameterInfo and FieldInfo overloads
- `EventInfoTests.cs` - Tests for EventInfo overloads (EventHandler, Action, Func, custom delegates)

## Development Notes

**Target Framework**: .NET 10.0 with implicit usings and nullable reference types enabled

**NullabilityInfoContext**: Used to extract nullability metadata from PropertyInfo, ParameterInfo, FieldInfo, and EventInfo for accurate nullable reference type detection

**Array Detection**: The code distinguishes between `IsVariableBoundArray` (multi-dimensional like `[,]`) and standard arrays (jagged like `[][]`)

**Type Erasure Limitation**: `typeof(List<string?>)` cannot preserve nullable reference type information at runtime due to type erasure. To get correct nullability for `List<string?>`:
- Use `property.ToReadableTypeString()` for properties
- Use `parameter.ToReadableTypeString()` for method parameters
- Use `field.ToReadableTypeString()` for fields
- Use `eventInfo.ToReadableTypeString()` for events
- Nullable **value types** (like `List<int?>`) work correctly with `typeof()` because `Nullable<T>` is a real runtime type

**EventInfo**: Events use their `EventHandlerType` property for type conversion. Works with standard event handlers (EventHandler, EventHandler<T>), Actions, Funcs, and custom delegates
