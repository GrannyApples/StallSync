using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StallSync.Models;

namespace StallSync.Data;

public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    private readonly bool _seedData;

    public AppDbContext(DbContextOptions<AppDbContext> options, bool seedData = true)
        : base(options)
    {
        _seedData = seedData;
    }

    public DbSet<TaskItem> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data (only if enabled)
        if (_seedData)
        {
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 1,
                    Title = "Utsläpp",
                    Description = "Släpp ut hästarna i hagen",
                    ResponsiblePerson = "Amanda Olsson",
                    StartDate = new DateTime(2024, 11, 21, 7, 0, 0),
                    IsCompleted = false
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Intag",
                    Description = "Ta in hästarna från hagen",
                    ResponsiblePerson = "Sara Wigren",
                    StartDate = new DateTime(2024, 11, 21, 17, 0, 0),
                    IsCompleted = false
                },
                new TaskItem
                {
                    Id = 3,
                    Title = "Fodring",
                    Description = "Fodra hästarna morgon och kväll",
                    ResponsiblePerson = "Nils Oscar",
                    StartDate = new DateTime(2024, 11, 21, 6, 0, 0),
                    IsCompleted = true
                }
            );
        }
    }
}
