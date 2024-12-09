namespace StallSync.Test;
using Xunit;
public class ValidatingHoursTest
{
    [Theory]
    [InlineData("2024-12-05T06:00", true)] 
    [InlineData("2024-12-05T19:00", true)] 
    [InlineData("2024-12-05T05:59", false)]
    [InlineData("2024-12-05T20:01", false)] 
    [InlineData("2024-12-05T15:30", true)]
    [InlineData("2024-12-05T00:00", false)]
    [InlineData("2024-12-06T06:00", true)]
    [InlineData("2024-12-05T20:00", false)]
    public void ValidateStartDate_WithinAllowedRange_ReturnsExpectedResult(string startDateString, bool expected)
    {
        // Arrange
        var startDate = DateTime.Parse(startDateString);
        var minAllowedTime = startDate.Date.AddHours(6); // 06:00
        var maxAllowedTime = startDate.Date.AddHours(19).AddMinutes(59); // 19:59

        // Act
        var isValid = startDate >= minAllowedTime && startDate <= maxAllowedTime;

        // Assert
        Assert.Equal(expected, isValid);
        
    }
}
