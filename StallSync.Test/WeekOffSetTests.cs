using StallSync.Data;
using StallSync.Models;

namespace StallSync.Test;

public class WeekOffSetTests : IClassFixture<AppDbContextFixture>
{
    private readonly AppDbContext _context;

    public WeekOffSetTests(AppDbContextFixture fixture)
    {
        _context = fixture.Context;
    }
    
    [Fact]
    public async Task GetTasksForWeek_ShouldReturnCorrectTasksBasedOnWeekOffset()
{
    // Arrange
    var today = new DateTime(2024, 11, 20); 
    var startOfCurrentWeek = today.Date.AddDays(-(int)today.DayOfWeek);

    // Seed tasks for the current week and adjacent weeks
    var tasks = new List<TaskItem>
    {
        new TaskItem
        {
            Id = 1,
            Title = "Current Week Task",
            StartDate = startOfCurrentWeek.AddHours(10),
            EndDate = startOfCurrentWeek.AddHours(12),
            IsCompleted = false
        },
        new TaskItem
        {
            Id = 2,
            Title = "Next Week Task",
            StartDate = startOfCurrentWeek.AddDays(7).AddHours(10),
            EndDate = startOfCurrentWeek.AddDays(7).AddHours(12),
            IsCompleted = false
        },
        new TaskItem
        {
            Id = 3,
            Title = "Previous Week Task",
            StartDate = startOfCurrentWeek.AddDays(-7).AddHours(10),
            EndDate = startOfCurrentWeek.AddDays(-7).AddHours(12),
            IsCompleted = false
        }
    };

    _context.TaskItems.AddRange(tasks);
    await _context.SaveChangesAsync();

    // Act: Fetch tasks for the current week
    var weekOffset = 0;
    var tasksForCurrentWeek = await GetTasksForWeekAsync(weekOffset);

    weekOffset = 1; // Next week
    var tasksForNextWeek = await GetTasksForWeekAsync(weekOffset);

    
    weekOffset = -1; // Previous week
    var tasksForPreviousWeek = await GetTasksForWeekAsync(weekOffset);

    // Assert
    Assert.Single(tasksForCurrentWeek);
    Assert.Equal("Current Week Task", tasksForCurrentWeek.First().Title);

    Assert.Single(tasksForNextWeek);
    Assert.Equal("Next Week Task", tasksForNextWeek.First().Title);

    Assert.Single(tasksForPreviousWeek);
    Assert.Equal("Previous Week Task", tasksForPreviousWeek.First().Title);
}
    
    
    private Task<List<TaskItem>> GetTasksForWeekAsync(int weekOffset)
    {
        var today = new DateTime(2024, 11, 20); // Mock "current" date
        var startOfWeek = today.Date.AddDays(-(int)today.DayOfWeek).AddDays(7 * weekOffset);
        var endOfWeek = startOfWeek.AddDays(7);

        return Task.FromResult(_context.TaskItems
            .Where(t => t.StartDate >= startOfWeek && t.StartDate < endOfWeek)
            .OrderBy(t => t.StartDate)
            .ToList());
    }
    
}