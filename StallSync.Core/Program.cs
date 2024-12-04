using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StallSync.Data;
using StallSync.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using StallSync.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//TODO: Lägg in rätt default connection( möjligtvis kör en SQLite istället för SSMS)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StallSyncConnection")));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();



builder.Services.AddSingleton<IEmailSender, MockEmailSender>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    // Applicera migrations och seed data
    context.Database.Migrate(); // Använder migrations
    await SeedData.SeedRolesAsync(services, userManager);
    // context.Database.EnsureCreated(); // Alternativ om du inte använder migrations
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
