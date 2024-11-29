using Microsoft.EntityFrameworkCore;
using StallSync.Models;

namespace StallSync.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options, bool seedData = true) : DbContext(options)
{
    private readonly bool _seedData;
    public DbSet<TaskItem> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Lägg till data seeding för projekt. (false in test)
        if(_seedData)
        {
        modelBuilder.Entity<TaskItem>().HasData(
            new TaskItem
            {
                Id = 1,
                Title = "Utsläpp",
                Description = "Släpp ut hästarna i hagen",
                ResponsiblePerson = "Amanda Olsson",
                StartDate = new DateTime(2024, 11, 21, 7, 0, 0),
                EndDate = new DateTime(2024, 11, 21, 8, 0, 0),
                IsCompleted = false
            },
            new TaskItem
            {
                Id = 2,
                Title = "Intag",
                Description = "Ta in hästarna från hagen",
                ResponsiblePerson = "Sara Wigren",
                StartDate = new DateTime(2024, 11, 21, 17, 0, 0),
                EndDate = new DateTime(2024, 11, 21, 18, 0, 0),
                IsCompleted = false
            },
            new TaskItem
            {
                Id = 3,
                Title = "Fodring",
                Description = "Fodra hästarna morgon och kväll",
                ResponsiblePerson = "Nils Oscar",
                StartDate = new DateTime(2024, 11, 21, 6, 0, 0),
                EndDate = new DateTime(2024, 11, 21, 19, 0, 0),
                IsCompleted = true
            }
        );
        }
    }
}