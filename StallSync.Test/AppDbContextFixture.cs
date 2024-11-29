using Microsoft.EntityFrameworkCore;
using StallSync.Data;


namespace StallSync.Test
{
    public class AppDbContextFixture : IDisposable
    {
        public AppDbContext Context { get; private set; }

        public AppDbContextFixture()
        {
            CreateContext();
            
            
        }

        public void CreateContext()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            // seedData set to false to not populate from .Core
            Context = new AppDbContext(options, seedData: false);
            ResetDatabase();
        }
        public void ResetDatabase()
        {
            Context.Database.EnsureDeleted(); // Remove existing data
            Context.Database.EnsureCreated(); // Recreate the schema
        }
        public void Dispose()
        {
          
            Context.Dispose();
        }
    }
}