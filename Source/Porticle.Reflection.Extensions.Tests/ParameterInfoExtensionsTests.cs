using System.Reflection;
using Porticle.Reflection.Extensions;
using static Porticle.Reflection.Extensions.Tests.TestFixtures;

namespace Porticle.Reflection.Extensions.Tests;

/// <summary>
/// Tests for the ParameterInfo extension method ToReadableTypeString(this ParameterInfo).
/// </summary>
public class ParameterInfoExtensionsTests
{
    #region Nullable Reference Types

    [Theory]
    [InlineData(0, "List<string>")]
    [InlineData(1, "List<string>?")]
    [InlineData(2, "List<int?>")]
    [InlineData(3, "List<string?>")]
    [InlineData(4, "List<string?>?")]
    public void ToReadableTypeString_ParameterInfo_ReturnsCorrectNullability(int parameterIndex, string expected)
    {
        var parameter = TestHelpers.GetParameter<MethodParametersClass>(
            nameof(MethodParametersClass.MethodWithParameters),
            parameterIndex);
        var result = parameter.ToReadableTypeString();
        Assert.Equal(expected, result);
    }

    #endregion

    #region UseFullNames Parameter

    [Fact]
    public void ToReadableTypeString_ParameterInfo_WithFullNames_ReturnsFullyQualifiedType()
    {
        var parameter = TestHelpers.GetParameter<MethodParametersClass>(
            nameof(MethodParametersClass.MethodWithParameters),
            1);
        var result = parameter.ToReadableTypeString(useFullNames: true);
        Assert.Equal("System.Collections.Generic.List<string>?", result);
    }

    #endregion

    #region Null Checks

    [Fact]
    public void ToReadableTypeString_ParameterInfo_NullParameter_ThrowsArgumentNullException()
    {
        ParameterInfo? nullParameter = null;
        Assert.Throws<ArgumentNullException>(() => nullParameter!.ToReadableTypeString());
    }

    #endregion

    #region Combined Parameters

    [Fact]
    public void ToReadableTypeString_ParameterInfo_AllParameters_WorkCorrectly()
    {
        var parameter = TestHelpers.GetParameter<MethodParametersClass>(
            nameof(MethodParametersClass.MethodWithParameters),
            4);
        var result = parameter.ToReadableTypeString(useFullNames: true, useInternalTypeNames: true);
        Assert.Equal("System.Collections.Generic.List<string?>?", result);
    }

    #endregion
}
