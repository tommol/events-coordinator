using FetishCompass.Domain.IAM.Model;

namespace FetishCompass.UnitTests.IAM.Domain;

public class UserProfileIdTests
{
    [Fact]
    public void New_GeneratesUniqueGuid()
    {
        var id1 = UserProfileId.New();
        var id2 = UserProfileId.New();
        Assert.NotEqual(id1, id2);
        Assert.NotEqual(id1.Value, Guid.Empty);
        Assert.NotEqual(id2.Value, Guid.Empty);
    }

    [Fact]
    public void ToString_ReturnsGuidString()
    {
        var guid = Guid.NewGuid();
        var id = new UserProfileId(guid);
        Assert.Equal(guid.ToString(), id.ToString());
    }

    [Fact]
    public void ImplicitGuidConversion_WorksBothWays()
    {
        var guid = Guid.NewGuid();
        UserProfileId id = (UserProfileId)guid;
        Guid guid2 = id;
        Assert.Equal(guid, id.Value);
        Assert.Equal(guid, guid2);
    }

    [Fact]
    public void Parse_StringAndSpan_Works()
    {
        var guid = Guid.NewGuid();
        var str = guid.ToString();
        var id1 = UserProfileId.Parse(str);
        var id2 = UserProfileId.Parse(str.AsSpan(), null);
        Assert.Equal(guid, id1.Value);
        Assert.Equal(guid, id2.Value);
    }

    [Fact]
    public void TryParse_StringAndSpan_ValidAndInvalid()
    {
        var guid = Guid.NewGuid();
        var str = guid.ToString();
        Assert.True(UserProfileId.TryParse(str, null, out var id1));
        Assert.Equal(guid, id1.Value);
        Assert.True(UserProfileId.TryParse(str.AsSpan(), null, out var id2));
        Assert.Equal(guid, id2.Value);
        Assert.False(UserProfileId.TryParse("not-a-guid", null, out _));
        Assert.False(UserProfileId.TryParse("not-a-guid".AsSpan(), null, out _));
    }

    [Fact]
    public void ToString_WithFormatProvider_DelegatesToGuid()
    {
        var guid = Guid.NewGuid();
        var id = new UserProfileId(guid);
        var formatted = id.ToString("N", null);
        Assert.Equal(guid.ToString("N", null), formatted);
    }

    [Fact]
    public void TryFormat_DelegatesToGuid()
    {
        var guid = Guid.NewGuid();
        var id = new UserProfileId(guid);
        Span<char> buffer = stackalloc char[32];
        var expected = guid.ToString("N");
        Assert.True(id.TryFormat(buffer, out var charsWritten, "N", null));
        Assert.Equal(expected, new string(buffer[..charsWritten]));
    }
}