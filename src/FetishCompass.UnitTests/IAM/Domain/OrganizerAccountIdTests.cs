using FetishCompass.Domain.IAM.Model;

namespace FetishCompass.UnitTests.IAM.Domain;

public class OrganizerAccountIdTests
    {
        [Fact]
        public void New_GeneratesUniqueGuid()
        {
            var id1 = OrganizerAccountId.New();
            var id2 = OrganizerAccountId.New();
            Assert.NotEqual(id1, id2);
            Assert.NotEqual(id1.Value, Guid.Empty);
            Assert.NotEqual(id2.Value, Guid.Empty);
        }

        [Fact]
        public void ToString_ReturnsGuidString()
        {
            var guid = Guid.NewGuid();
            var id = new OrganizerAccountId(guid);
            Assert.Equal(guid.ToString(), id.ToString());
        }

        [Fact]
        public void ImplicitGuidConversion_WorksBothWays()
        {
            var guid = Guid.NewGuid();
            OrganizerAccountId id = (OrganizerAccountId)guid;
            Guid guid2 = id;
            Assert.Equal(guid, id.Value);
            Assert.Equal(guid, guid2);
        }

        [Fact]
        public void Parse_StringAndSpan_Works()
        {
            var guid = Guid.NewGuid();
            var str = guid.ToString();
            var id1 = OrganizerAccountId.Parse(str);
            var id2 = OrganizerAccountId.Parse(str.AsSpan(), null);
            Assert.Equal(guid, id1.Value);
            Assert.Equal(guid, id2.Value);
        }

        [Fact]
        public void TryParse_StringAndSpan_ValidAndInvalid()
        {
            var guid = Guid.NewGuid();
            var str = guid.ToString();
            Assert.True(OrganizerAccountId.TryParse(str, null, out var id1));
            Assert.Equal(guid, id1.Value);
            Assert.True(OrganizerAccountId.TryParse(str.AsSpan(), null, out var id2));
            Assert.Equal(guid, id2.Value);
            Assert.False(OrganizerAccountId.TryParse("not-a-guid", null, out _));
            Assert.False(OrganizerAccountId.TryParse("not-a-guid".AsSpan(), null, out _));
        }

        [Fact]
        public void ToString_WithFormatProvider_DelegatesToGuid()
        {
            var guid = Guid.NewGuid();
            var id = new OrganizerAccountId(guid);
            var formatted = id.ToString("N", null);
            Assert.Equal(guid.ToString("N", null), formatted);
        }

        [Fact]
        public void TryFormat_DelegatesToGuid()
        {
            var guid = Guid.NewGuid();
            var id = new OrganizerAccountId(guid);
            Span<char> buffer = stackalloc char[32];
            var expected = guid.ToString("N");
            Assert.True(id.TryFormat(buffer, out var charsWritten, "N", null));
            Assert.Equal(expected, new string(buffer[..charsWritten]));
        }
    }