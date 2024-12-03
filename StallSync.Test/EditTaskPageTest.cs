using Microsoft.AspNetCore.Mvc;
using StallSync.Data;
using StallSync.Models;
using StallSync.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StallSync.Test
{
    public class EditTaskPageTest : IClassFixture<AppDbContextFixture>
    {
        private readonly AppDbContext _context;

        public EditTaskPageTest(AppDbContextFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task OnPostAsync_UpdatesTaskWithCorrectEndDate()
        {
            // Arrange
            var originalTask = new TaskItem
            {
                Id = 1,
                Title = "Original Task",
                Description = "Original Description",
                ResponsiblePerson = "Original Person",
                StartDate = DateTime.Now,
                IsCompleted = false
            };

            _context.TaskItems.Add(originalTask);
            await _context.SaveChangesAsync();

            var updatedTaskData = new TaskItem
            {
                Id = 1,
                Title = "Updated Task",
                Description = "Updated Description",
                ResponsiblePerson = "Updated Person",
                StartDate = originalTask.StartDate.AddDays(1), // Ny start-tid
                IsCompleted = true
            };

            var pageModel = new EditTaskPageModel(_context)
            {
                Task = updatedTaskData
            };

            // Act
            var result = await pageModel.OnPostAsync(updatedTaskData.Id);

            // Assert
            Assert.IsType<RedirectToPageResult>(result);

            var taskFromDb = await _context.TaskItems.FindAsync(originalTask.Id);
            Assert.NotNull(taskFromDb);
            Assert.Equal("Updated Task", taskFromDb!.Title);
            Assert.Equal("Updated Description", taskFromDb.Description);
            Assert.Equal("Updated Person", taskFromDb.ResponsiblePerson);
            Assert.Equal(updatedTaskData.StartDate, taskFromDb.StartDate);
            Assert.True(taskFromDb.IsCompleted);
        }
    }
}
