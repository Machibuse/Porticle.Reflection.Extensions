using Porticle.Reflection.Extensions;

namespace Porticle.Reflection.Extensions.Tests;

/// <summary>
/// Tests for the Type extension method ToReadableTypeString(this Type).
/// </summary>
public class TypeExtensionsTests
{
    #region Primitive Types and Type Aliases

    [Theory]
    [InlineData(typeof(int), true, "int")]
    [InlineData(typeof(int), false, "Int32")]
    [InlineData(typeof(string), true, "string")]
    [InlineData(typeof(string), false, "String")]
    [InlineData(typeof(bool), true, "bool")]
    [InlineData(typeof(bool), false, "Boolean")]
    [InlineData(typeof(double), true, "double")]
    [InlineData(typeof(double), false, "Double")]
    [InlineData(typeof(decimal), true, "decimal")]
    [InlineData(typeof(decimal), false, "Decimal")]
    [InlineData(typeof(char), true, "char")]
    [InlineData(typeof(char), false, "Char")]
    [InlineData(typeof(object), true, "object")]
    [InlineData(typeof(object), false, "Object")]
    [InlineData(typeof(byte), true, "byte")]
    [InlineData(typeof(byte), false, "Byte")]
    [InlineData(typeof(sbyte), true, "sbyte")]
    [InlineData(typeof(sbyte), false, "SByte")]
    [InlineData(typeof(short), true, "short")]
    [InlineData(typeof(short), false, "Int16")]
    [InlineData(typeof(ushort), true, "ushort")]
    [InlineData(typeof(ushort), false, "UInt16")]
    [InlineData(typeof(uint), true, "uint")]
    [InlineData(typeof(uint), false, "UInt32")]
    [InlineData(typeof(long), true, "long")]
    [InlineData(typeof(long), false, "Int64")]
    [InlineData(typeof(ulong), true, "ulong")]
    [InlineData(typeof(ulong), false, "UInt64")]
    [InlineData(typeof(float), true, "float")]
    [InlineData(typeof(float), false, "Single")]
    [InlineData(typeof(nint), true, "nint")]
    [InlineData(typeof(nint), false, "IntPtr")]
    [InlineData(typeof(nuint), true, "nuint")]
    [InlineData(typeof(nuint), false, "UIntPtr")]
    public void ToReadableTypeString_PrimitiveTypes_ReturnsCorrectAlias(Type type, bool useInternalTypeNames, string expected)
    {
        var result = type.ToReadableTypeString(useInternalTypeNames: useInternalTypeNames);
        Assert.Equal(expected, result);
    }

    #endregion

    #region Nullable Value Types

    [Theory]
    [InlineData(typeof(int?), "int?")]
    [InlineData(typeof(bool?), "bool?")]
    [InlineData(typeof(decimal?), "decimal?")]
    [InlineData(typeof(DateTime?), "DateTime?")]
    [InlineData(typeof(double?), "double?")]
    [InlineData(typeof(char?), "char?")]
    public void ToReadableTypeString_NullableValueTypes_ReturnsTypeWithQuestionMark(Type type, string expected)
    {
        var result = type.ToReadableTypeString();
        Assert.Equal(expected, result);
    }

    #endregion

    #region Arrays

    [Theory]
    [InlineData(typeof(int[]), "int[]")]
    [InlineData(typeof(string[]), "string[]")]
    [InlineData(typeof(int[][]), "int[][]")]
    [InlineData(typeof(int[][][]), "int[][][]")]
    [InlineData(typeof(int[,]), "int[,]")]
    [InlineData(typeof(int[,,]), "int[,,]")]
    [InlineData(typeof(int?[]), "int?[]")]
    [InlineData(typeof(List<int>[]), "List<int>[]")]
    public void ToReadableTypeString_Arrays_ReturnsCorrectArrayNotation(Type type, string expected)
    {
        var result = type.ToReadableTypeString();
        Assert.Equal(expected, result);
    }

    #endregion

    #region Generic Types

    [Theory]
    [InlineData(typeof(List<int>), "List<int>")]
    [InlineData(typeof(List<string>), "List<string>")]
    [InlineData(typeof(Dictionary<string, int>), "Dictionary<string,int>")]
    [InlineData(typeof(List<List<int>>), "List<List<int>>")]
    [InlineData(typeof(Dictionary<string, List<int>>), "Dictionary<string,List<int>>")]
    [InlineData(typeof(List<int?>), "List<int?>")]
    [InlineData(typeof(Dictionary<string, int?>), "Dictionary<string,int?>")]
    [InlineData(typeof(KeyValuePair<string, int>), "KeyValuePair<string,int>")]
    [InlineData(typeof(Task<int>), "Task<int>")]
    [InlineData(typeof(List<int[]>), "List<int[]>")]
    public void ToReadableTypeString_GenericTypes_ReturnsCorrectFormat(Type type, string expected)
    {
        var result = type.ToReadableTypeString();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToReadableTypeString_ListOfNullableString_TypeErasure_ReturnsListString()
    {
        // Note: typeof(List<string?>) suffers from type erasure - nullable reference types
        // are compile-time only. At runtime, this is identical to typeof(List<string>).
        // To preserve nullability info, use PropertyInfo, ParameterInfo, or FieldInfo instead.
        var result = typeof(List<string?>).ToReadableTypeString();
        Assert.Equal("List<string>", result); // No "?" because of type erasure
    }

    #endregion

    #region UseFullNames Parameter

    [Theory]
    [InlineData(typeof(List<int>), true, true, "System.Collections.Generic.List<int>")]
    [InlineData(typeof(List<int>), false, true, "List<int>")]
    [InlineData(typeof(List<int>), true, false, "System.Collections.Generic.List<System.Int32>")]
    [InlineData(typeof(List<int>), false, false, "List<Int32>")]
    [InlineData(typeof(Dictionary<string, int>), true, true, "System.Collections.Generic.Dictionary<string,int>")]
    [InlineData(typeof(Dictionary<string, int>), false, true, "Dictionary<string,int>")]
    [InlineData(typeof(Dictionary<string, int>), true, false, "System.Collections.Generic.Dictionary<System.String,System.Int32>")]
    [InlineData(typeof(Dictionary<string, int>), false, false, "Dictionary<String,Int32>")]
    [InlineData(typeof(int), true, true, "int")]
    [InlineData(typeof(int), false, true, "int")]
    [InlineData(typeof(int), true, false, "System.Int32")]
    [InlineData(typeof(int), false, false, "Int32")]
    public void ToReadableTypeString_UseFullNames_ReturnsFullyQualifiedNames(Type type, bool useFullNames, bool useInternalTypeNames, string expected)
    {
        var result = type.ToReadableTypeString(useFullnames: useFullNames, useInternalTypeNames: useInternalTypeNames);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToReadableTypeString_CustomType_WithFullNames_ReturnsFullyQualifiedName()
    {
        var result = typeof(TestFixtures.NullablePropertiesClass).ToReadableTypeString(useFullnames: true);
        Assert.Equal("Porticle.Reflection.Extensions.Tests.TestFixtures+NullablePropertiesClass", result);
    }

    [Fact]
    public void ToReadableTypeString_CustomType_WithoutFullNames_ReturnsShortName()
    {
        var result = typeof(TestFixtures.NullablePropertiesClass).ToReadableTypeString(useFullnames: false);
        Assert.Equal("NullablePropertiesClass", result);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ToReadableTypeString_NullType_ThrowsArgumentNullException()
    {
        Type? nullType = null;
        Assert.Throws<ArgumentNullException>(() => nullType!.ToReadableTypeString());
    }

    [Fact]
    public void ToReadableTypeString_VoidType_ReturnsVoid()
    {
        var result = typeof(void).ToReadableTypeString();
        Assert.Equal("Void", result);
    }

    #endregion

    #region Combined Parameters

    [Fact]
    public void ToReadableTypeString_AllParameters_WorkCorrectly()
    {
        var result = typeof(List<int>).ToReadableTypeString(
            nullabilityInfo: null,
            useFullnames: true,
            useInternalTypeNames: true);
        Assert.Equal("System.Collections.Generic.List<int>", result);
    }

    [Fact]
    public void ToReadableTypeString_IntWithFullNamesAndNoInternalNames_ReturnsSystemInt32()
    {
        var result = typeof(int).ToReadableTypeString(useInternalTypeNames: false, useFullnames: true);
        Assert.Equal("System.Int32", result);
    }

    #endregion
}
