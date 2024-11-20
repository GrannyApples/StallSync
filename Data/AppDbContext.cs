using Microsoft.EntityFrameworkCore;
using StallSync.Models;

namespace StallSync.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TaskItem> TaskItems { get; set; }
}
