namespace StallSync.Test;

using StallSync.Models;
using System.ComponentModel.DataAnnotations;
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
        var sut = new TaskItem { StartDate = startDate };

        // Act
        var isValid =
            sut.StartDate.TimeOfDay >= TimeSpan.FromHours(6) && 
            sut.StartDate.TimeOfDay <= TimeSpan.FromHours(19).Add(TimeSpan.FromMinutes(59));

        // Assert
        Assert.Equal(expected, isValid);
        
    }
}
