using Microsoft.EntityFrameworkCore;
using StallSync.Data;


namespace StallSync.Test
{
    public class AppDbContextFixture : IDisposable
    {
        public AppDbContext Context { get; private set; }

        public AppDbContextFixture()
        {
        
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            // seedData set to false to not populate from .Core
            Context = new AppDbContext(options, seedData: false);

          
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
          
            Context.Dispose();
        }
    }
}