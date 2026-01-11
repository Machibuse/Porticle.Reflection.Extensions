using System.Reflection;
using Porticle.Reflection.Extensions;
using static Porticle.Reflection.Extensions.Tests.TestFixtures;

namespace Porticle.Reflection.Extensions.Tests;

/// <summary>
/// Tests for the PropertyInfo extension method ToReadableTypeString(this PropertyInfo).
/// </summary>
public class PropertyInfoExtensionsTests
{
    #region Nullable Reference Types

    [Theory]
    [InlineData(nameof(NullablePropertiesClass.NonNullableString), "string")]
    [InlineData(nameof(NullablePropertiesClass.NullableString), "string?")]
    [InlineData(nameof(NullablePropertiesClass.NonNullableInt), "int")]
    [InlineData(nameof(NullablePropertiesClass.NullableInt), "int?")]
    [InlineData(nameof(NullablePropertiesClass.NonNullableList), "List<string>")]
    [InlineData(nameof(NullablePropertiesClass.NullableList), "List<string>?")]
    [InlineData(nameof(NullablePropertiesClass.ListOfNullableInt), "List<int?>")]
    [InlineData(nameof(NullablePropertiesClass.ListOfNullableString), "List<string?>")]
    [InlineData(nameof(NullablePropertiesClass.NullableListOfNullableInt), "List<int?>?")]
    [InlineData(nameof(NullablePropertiesClass.NullableListOfNullableString), "List<string?>?")]
    public void ToReadableTypeString_PropertyInfo_ReturnsCorrectNullability(string propertyName, string expected)
    {
        var property = TestHelpers.GetProperty<NullablePropertiesClass>(propertyName);
        var result = property.ToReadableTypeString();
        Assert.Equal(expected, result);
    }

    #endregion

    #region UseFullNames Parameter

    [Fact]
    public void ToReadableTypeString_PropertyInfo_WithFullNames_ReturnsFullyQualifiedType()
    {
        var property = TestHelpers.GetProperty<NullablePropertiesClass>(nameof(NullablePropertiesClass.NullableString));
        var result = property.ToReadableTypeString(useFullNames: true, useInternalTypeNames: true);
        Assert.Equal("string?", result); // string is a primitive, so no full name even with useFullNames: true
    }

    [Fact]
    public void ToReadableTypeString_PropertyInfo_GenericWithFullNames_ReturnsFullyQualifiedType()
    {
        var property = TestHelpers.GetProperty<NullablePropertiesClass>(nameof(NullablePropertiesClass.NullableList));
        var result = property.ToReadableTypeString(useFullNames: true);
        Assert.Equal("System.Collections.Generic.List<string>?", result);
    }

    #endregion

    #region Null Checks

    [Fact]
    public void ToReadableTypeString_PropertyInfo_NullProperty_ThrowsArgumentNullException()
    {
        PropertyInfo? nullProperty = null;
        Assert.Throws<ArgumentNullException>(() => nullProperty!.ToReadableTypeString());
    }

    #endregion

    #region Combined Parameters

    [Fact]
    public void ToReadableTypeString_PropertyInfo_AllParameters_WorkCorrectly()
    {
        var property = TestHelpers.GetProperty<NullablePropertiesClass>(nameof(NullablePropertiesClass.NullableString));
        var result = property.ToReadableTypeString(useFullNames: true, useInternalTypeNames: true);
        Assert.Equal("string?", result);
    }

    #endregion
}
