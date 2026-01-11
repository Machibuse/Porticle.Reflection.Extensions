using System.Reflection;
using Porticle.Reflection.Extensions;
using static Porticle.Reflection.Extensions.Tests.TestFixtures;

namespace Porticle.Reflection.Extensions.Tests;

/// <summary>
/// Tests for the FieldInfo extension method ToReadableTypeString(this FieldInfo).
/// </summary>
public class FieldInfoExtensionsTests
{
    #region Nullable Reference Types

    [Theory]
    [InlineData(nameof(NullableFieldsClass.NonNullableList), "List<string>")]
    [InlineData(nameof(NullableFieldsClass.NullableList), "List<string>?")]
    [InlineData(nameof(NullableFieldsClass.ListOfNullableInt), "List<int?>")]
    [InlineData(nameof(NullableFieldsClass.ListOfNullableString), "List<string?>")]
    [InlineData(nameof(NullableFieldsClass.NullableListOfNullableString), "List<string?>?")]
    public void ToReadableTypeString_FieldInfo_ReturnsCorrectNullability(string fieldName, string expected)
    {
        var field = TestHelpers.GetField<NullableFieldsClass>(fieldName);
        var result = field.ToReadableTypeString();
        Assert.Equal(expected, result);
    }

    #endregion

    #region UseFullNames Parameter

    [Fact]
    public void ToReadableTypeString_FieldInfo_WithFullNames_ReturnsFullyQualifiedType()
    {
        var field = TestHelpers.GetField<NullableFieldsClass>(nameof(NullableFieldsClass.NullableList));
        var result = field.ToReadableTypeString(useFullNames: true);
        Assert.Equal("System.Collections.Generic.List<string>?", result);
    }

    #endregion

    #region Null Checks

    [Fact]
    public void ToReadableTypeString_FieldInfo_NullField_ThrowsArgumentNullException()
    {
        FieldInfo? nullField = null;
        Assert.Throws<ArgumentNullException>(() => nullField!.ToReadableTypeString());
    }

    #endregion

    #region Combined Parameters

    [Fact]
    public void ToReadableTypeString_FieldInfo_AllParameters_WorkCorrectly()
    {
        var field = TestHelpers.GetField<NullableFieldsClass>(nameof(NullableFieldsClass.NullableListOfNullableString));
        var result = field.ToReadableTypeString(useFullNames: true, useInternalTypeNames: true);
        Assert.Equal("System.Collections.Generic.List<string?>?", result);
    }

    #endregion
}
