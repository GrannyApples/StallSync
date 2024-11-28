using Microsoft.EntityFrameworkCore;
using StallSync.Data;


namespace StallSync.Test
{
    public class AppDbContextFixture : IDisposable
    {
        public AppDbContext Context { get; private set; }

        public AppDbContextFixture()
        {
            // Setup InMemoryDatabase
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            Context = new AppDbContext(options);

            // Se till att databasen är tom innan varje test körs
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            // Rensa upp när testerna är klara
            Context.Dispose();
        }
    }
}