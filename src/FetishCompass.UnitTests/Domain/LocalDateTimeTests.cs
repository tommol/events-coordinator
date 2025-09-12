using FetishCompass.Domain;
using System;
using Xunit;

namespace FetishCompass.UnitTests.Domain
{
    public class LocalDateTimeTests
    {
        [Fact]
        public void Create_ValidInput_ReturnsLocalDateTime()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var time = TimeOnly.FromDateTime(DateTime.UtcNow);
            var timeZone = "Europe/Warsaw";

            // Act
            var localDateTime = LocalDateTime.Create(date, time, timeZone);

            // Assert
            Assert.NotNull(localDateTime);
            Assert.Equal(date, localDateTime.Date);
            Assert.Equal(time, localDateTime.Time);
            Assert.Equal(timeZone, localDateTime.TimeZoneId);
        }
        
        [Fact]
        public void Equals_SameValues_ReturnsTrue()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var time = TimeOnly.FromDateTime(DateTime.UtcNow);
            var timeZone = "Europe/Warsaw";
            var localDateTime1 = LocalDateTime.Create(date, time, timeZone);
            var localDateTime2 = LocalDateTime.Create(date, time, timeZone);

            // Act
            var areEqual = localDateTime1.Equals(localDateTime2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_DifferentValues_ReturnsFalse()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var time = TimeOnly.FromDateTime(DateTime.UtcNow);
            var timeZone = "Europe/Warsaw";
            var localDateTime1 = LocalDateTime.Create(date, time, timeZone);
            var localDateTime2 = LocalDateTime.Create(date, time.AddHours(1), timeZone);

            // Act
            var areEqual = localDateTime1.Equals(localDateTime2);

            // Assert
            Assert.False(areEqual);
        }
    }
}
