using System.Reflection;
using Porticle.Reflection.Extensions;
using static Porticle.Reflection.Extensions.Tests.TestFixtures;

namespace Porticle.Reflection.Extensions.Tests;

/// <summary>
/// Tests for the EventInfo extension method ToReadableTypeString(this EventInfo).
/// </summary>
public class EventInfoExtensionsTests
{
    #region Standard EventHandler Tests

    [Theory]
    [InlineData(nameof(EventsClass.StandardEvent), "EventHandler?")]
    [InlineData(nameof(EventsClass.NonNullableEvent), "EventHandler")]
    public void ToReadableTypeString_EventInfo_StandardEventHandler_ReturnsCorrectFormat(string eventName, string expected)
    {
        var eventInfo = TestHelpers.GetEvent<EventsClass>(eventName);
        var result = eventInfo.ToReadableTypeString();
        Assert.Equal(expected, result);
    }

    #endregion

    #region Custom Delegate Tests

    [Theory]
    [InlineData(nameof(EventsClass.SimpleEvent), "SimpleEventHandler?")]
    [InlineData(nameof(EventsClass.StringEvent), "StringEventHandler?")]
    public void ToReadableTypeString_EventInfo_CustomDelegates_ReturnsCorrectFormat(string eventName, string expected)
    {
        var eventInfo = TestHelpers.GetEvent<EventsClass>(eventName);
        var result = eventInfo.ToReadableTypeString();
        Assert.Equal(expected, result);
    }

    #endregion

    #region Generic EventHandler Tests

    [Theory]
    [InlineData(nameof(EventsClass.GenericStringEvent), "EventHandler<string>?")]
    [InlineData(nameof(EventsClass.GenericIntEvent), "EventHandler<int>?")]
    [InlineData(nameof(EventsClass.GenericListEvent), "EventHandler<List<string>>?")]
    [InlineData(nameof(EventsClass.GenericListOfNullableStringEvent), "EventHandler<List<string?>>?")]
    [InlineData(nameof(EventsClass.GenericListOfNullableIntEvent), "EventHandler<List<int?>>?")]
    public void ToReadableTypeString_EventInfo_GenericEventHandlers_ReturnsCorrectFormat(string eventName, string expected)
    {
        var eventInfo = TestHelpers.GetEvent<EventsClass>(eventName);
        var result = eventInfo.ToReadableTypeString();
        Assert.Equal(expected, result);
    }

    #endregion

    #region Action and Func Tests

    [Theory]
    [InlineData(nameof(EventsClass.ActionEvent), "Action?")]
    [InlineData(nameof(EventsClass.ActionStringEvent), "Action<string>?")]
    [InlineData(nameof(EventsClass.ActionNullableStringEvent), "Action<string?>?")]
    [InlineData(nameof(EventsClass.FuncIntEvent), "Func<int>?")]
    [InlineData(nameof(EventsClass.FuncNullableStringEvent), "Func<string?>?")]
    public void ToReadableTypeString_EventInfo_ActionAndFunc_ReturnsCorrectFormat(string eventName, string expected)
    {
        var eventInfo = TestHelpers.GetEvent<EventsClass>(eventName);
        var result = eventInfo.ToReadableTypeString();
        Assert.Equal(expected, result);
    }

    #endregion

    #region UseFullNames Tests

    [Theory]
    [InlineData(nameof(EventsClass.StandardEvent), "System.EventHandler?")]
    [InlineData(nameof(EventsClass.GenericStringEvent), "System.EventHandler<string>?")]
    [InlineData(nameof(EventsClass.ActionEvent), "System.Action?")]
    public void ToReadableTypeString_EventInfo_WithFullNames_ReturnsFullyQualifiedNames(string eventName, string expected)
    {
        var eventInfo = TestHelpers.GetEvent<EventsClass>(eventName);
        var result = eventInfo.ToReadableTypeString(useFullNames: true);
        Assert.Equal(expected, result);
    }

    #endregion

    #region Null Checks

    [Fact]
    public void ToReadableTypeString_EventInfo_NullEventInfo_ThrowsArgumentNullException()
    {
        EventInfo? nullEvent = null;
        Assert.Throws<ArgumentNullException>(() => nullEvent!.ToReadableTypeString());
    }

    #endregion
}
