using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StallSync.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace StallSync.Test
{
    public class IdentityIntegrationTests : IClassFixture<AppDbContextFixture>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityIntegrationTests(AppDbContextFixture fixture)
        {
            _context = fixture.Context;

            // Skapa en UserManager för att hantera IdentityUser
            var store = new UserStore<IdentityUser>(_context);
            _userManager = new UserManager<IdentityUser>(
                store,
                null, // Ingen specifik IOptions
                new PasswordHasher<IdentityUser>(),
                null, // Ingen specifik UserValidator
                null, // Ingen specifik PasswordValidator
                null, // Ingen specifik LookupNormalizer
                null, // Ingen specifik IdentityErrorDescriber
                null, // Ingen specifik ServiceProvider
                NullLogger<UserManager<IdentityUser>>.Instance  // Här behövs en NullLogger.
            );
        }

        [Fact]
        public async Task RegisterUser_AddsUserToDatabase()
        {
            // Arrange
            var email = "testuser@example.com";
            var password = "TestPassword123!";
            var user = new IdentityUser { UserName = email, Email = email };

            // Act
            var result = await _userManager.CreateAsync(user, password);

            // Assert
            Assert.True(result.Succeeded);
            var createdUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            Assert.NotNull(createdUser);
            Assert.Equal(email, createdUser.Email);
        }

        [Fact]
        public async Task LoginUser_ReturnsTrue_WhenCredentialsAreCorrect()
        {
            // Arrange
            var email = "loginuser@example.com";
            var password = "LoginPassword123!";
            var user = new IdentityUser { UserName = email, Email = email };

            // Skapa användaren
            await _userManager.CreateAsync(user, password);

            // Act
            var passwordCheck = await _userManager.CheckPasswordAsync(user, password);

            // Assert
            Assert.True(passwordCheck);
        }

        [Fact]
        public async Task LoginUser_ReturnsFalse_WhenCredentialsAreIncorrect()
        {
            // Arrange
            var email = "wronglogin@example.com";
            var password = "CorrectPassword123!";
            var wrongPassword = "WrongPassword123!";
            var user = new IdentityUser { UserName = email, Email = email };

            // Skapa användaren
            await _userManager.CreateAsync(user, password);

            // Act
            var passwordCheck = await _userManager.CheckPasswordAsync(user, wrongPassword);

            // Assert
            Assert.False(passwordCheck);
        }
    }
}