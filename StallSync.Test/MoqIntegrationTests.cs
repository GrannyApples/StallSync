using StallSync.Models;
using StallSync.Data;
using Microsoft.EntityFrameworkCore;

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
        public async Task GetTaskItems_ShouldReturnMockedData()
        {
            // Arrange: Lägg till testdata i databasen
            var taskItems = new[]
            {
                new TaskItem
                {
                    Id = 9,
                    Title = "Utsläpp",
                    Description = "Släpp ut hästarna i hagen",
                    ResponsiblePerson = "Amanda Olsson",
                    StartDate = new DateTime(2024, 11, 21, 7, 0, 0),
                    EndDate = new DateTime(2024, 11, 21, 8, 0, 0),
                    IsCompleted = false
                },
                new TaskItem
                {
                    Id = 12,
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

            // Act: Hämta data från databasen
            var result = await _context.TaskItems.ToListAsync();

            // Assert: Verifiera att datan är korrekt
            Assert.Equal(5, result.Count);
            Assert.Equal("Utsläpp", result[0].Title);
            Assert.Equal("Intag", result[1].Title);
        }
    }
}