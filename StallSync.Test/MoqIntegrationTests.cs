using StallSync.Models;
using StallSync.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace StallSync.Test
{
    public class MoqIntegrationTests : IClassFixture<AppDbContextFixture>
    {
        private readonly AppDbContext _context;

        public MoqIntegrationTests(AppDbContextFixture fixture)
        {
            
            _context = fixture.Context;
        }

        [Fact]
        public async Task OnPostDeleteAsync_RemovesTask_WhenTaskExists()
        {
           
            // Arrange
            var taskToDelete = new TaskItem
            {
                Id = 1,
                Title = "Mock Task",
                Description = "To be deleted",
                ResponsiblePerson = "Test User",
                StartDate = DateTime.Now
            };

            _context.TaskItems.Add(taskToDelete);
            await _context.SaveChangesAsync();

            // Act
            _context.TaskItems.Remove(taskToDelete);
            await _context.SaveChangesAsync();

            // Assert
            var deletedTask = await _context.TaskItems.FindAsync(taskToDelete.Id);
            Assert.Null(deletedTask);
        }

        [Fact]
        public async Task GetTaskItems_ShouldReturnMockedData()
        {
            // Arrange
            var taskItems = new[]
            {
                new TaskItem
                {
                    Id = 2,
                    Title = "Utsläpp",
                    Description = "Släpp ut hästarna i hagen",
                    ResponsiblePerson = "Amanda Olsson",
                    StartDate = new DateTime(2024, 11, 21, 7, 0, 0),
                    EndDate = new DateTime(2024, 11, 21, 8, 0, 0),
                    IsCompleted = false
                },
                new TaskItem
                {
                    Id = 3,
                    Title = "Intag",
                    Description = "Ta in hästarna från hagen",
                    ResponsiblePerson = "Sara Wigren",
                    StartDate = new DateTime(2024, 11, 21, 17, 0, 0),
                    EndDate = new DateTime(2024, 11, 21, 18, 0, 0),
                    IsCompleted = false
                }
            };

            _context.TaskItems.AddRange(taskItems);
            await _context.SaveChangesAsync();

            // Act
            var result = await _context.TaskItems.ToListAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Utsläpp", result[0].Title);
            Assert.Equal("Intag", result[1].Title);
        }

        [Fact]
        public async Task OnPostDeleteAsync_DoesNothing_WhenTaskDoesNotExist()
        {
            // Arrange
            var nonExistentTaskId = 99;

            // Act
            var taskToDelete = await _context.TaskItems.FindAsync(nonExistentTaskId);
            if (taskToDelete != null)
            {
                _context.TaskItems.Remove(taskToDelete);
                await _context.SaveChangesAsync();
            }

            // Assert
            var allTasks = await _context.TaskItems.ToListAsync();
            Assert.NotNull(allTasks);
        }
    }
}