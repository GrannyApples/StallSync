using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StallSync.Data;
using StallSync.Models;
using StallSync.Pages.Shared;
public class HorsePicPageTests
{
    private string UploadFolderPath => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
        return new AppDbContext(options);
    }

    private HorsePicPageModel CreatePageModel(AppDbContext context)
    {
        return new HorsePicPageModel(context);
    }

    public HorsePicPageTests()
    {
        if (!Directory.Exists(UploadFolderPath))
        {
            Directory.CreateDirectory(UploadFolderPath);
        }
    }

    [Fact]
    public void Test_Horse_Pic_Creation_Saves_File_Correctly()
    {
        // Arrange
        var context = CreateDbContext();
        var pageModel = CreatePageModel(context);

        var file = new FormFile(new MemoryStream(new byte[256]), 0, 256, "test", "test.jpg");
        pageModel.UploadedFile = file;
        pageModel.HorseName = "Horse 1";

        // Act
        var result = pageModel.OnPostAsync().Result;

        // Assert
        var horse = context.Horses.Find(1);
        Assert.NotNull(horse);
        Assert.Equal("Horse 1", horse.Name);
        Assert.StartsWith("/uploads/", horse.ImagePath);
    }

    [Fact]
    public void Test_Horses_Are_Shown_After_Upload()
    {
        // Arrange
        var context = CreateDbContext();
        var pageModel = CreatePageModel(context);

        var horse1 = new Horse { Name = "Horse 1", ImagePath = "/uploads/horse1.jpg" };
        var horse2 = new Horse { Name = "Horse 2", ImagePath = "/uploads/horse2.jpg" };
        context.Horses.AddRange(horse1, horse2);
        context.SaveChanges();

        // Act
        pageModel.OnGet();

        // Assert
        Assert.Contains(pageModel.Horses, h => h.Name == "Horse 1");
        Assert.Contains(pageModel.Horses, h => h.Name == "Horse 2");
    }

    [Fact]
    public async Task Test_Delete_Horse_Removes_From_Database_And_File()
    {
        // Arrange
        var context = CreateDbContext();
        var pageModel = CreatePageModel(context);

        var horse = new Horse { Name = "Horse 1", ImagePath = "/uploads/horse1.jpg" };
        context.Horses.Add(horse);
        await context.SaveChangesAsync();

        var filePath = Path.Combine(UploadFolderPath, "horse1.jpg");
        await File.WriteAllTextAsync(filePath, "dummy content");

        // Act
        var result = await pageModel.OnPostDeleteAsync(horse.Id);

        // Assert
        Assert.Null(context.Horses.Find(horse.Id));
        Assert.False(File.Exists(filePath));
    }

    [Fact]
    public async Task Test_Delete_Horse_Returns_NotFound_If_Horse_Not_Exists()
    {
        // Arrange
        var context = CreateDbContext();
        var pageModel = CreatePageModel(context);

        // Act
        var result = await pageModel.OnPostDeleteAsync(999); 

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}