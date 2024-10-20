using Microsoft.EntityFrameworkCore;
using PasswordManagerApi.Data;
using PasswordManagerApi.Models;
using PasswordManagerApi.Services;
using System.Text;
using Xunit;

namespace PasswordManagerApi.Tests
{
    public class PasswordServiceTests
    {
        private readonly PasswordService _service;
        private readonly DataContext _context;

        public PasswordServiceTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new DataContext(options);
            _service = new PasswordService(_context);
        }

        [Fact]
        public async Task AddPasswordAsync_ShouldAddPasswordWithBase64Encryption()
        {
            var password = new Password { UserName = "user1", App = "app1", Category = "category1", EncryptedPassword = "myPassword" };
            var result = await _service.AddPasswordAsync(password);
            var savedPassword = await _context.Passwords.FindAsync(result.Id);
            Assert.NotNull(savedPassword);
            Assert.Equal("myPassword", Encoding.ASCII.GetString(Convert.FromBase64String(savedPassword.EncryptedPassword)));
        }

        [Fact]
        public async Task GetPasswordsAsync_ShouldReturnAllPasswords()
        {
            await _service.AddPasswordAsync(new Password { UserName = "user1", App = "app1", Category = "category1", EncryptedPassword = "myPassword" });
            await _service.AddPasswordAsync(new Password { UserName = "user2", App = "app2", Category = "category2", EncryptedPassword = "myPassword2" });
            var passwords = await _service.GetPasswordsAsync();
            Assert.Equal(2, passwords.Count());
        }

        [Fact]
        public async Task GetPasswordByIdAsync_ShouldReturnPassword_IfExists_WithDecryption()
        {
            var originalPassword = "myPassword";
            var encryptedPassword = Convert.ToBase64String(Encoding.ASCII.GetBytes(originalPassword));
            var password = await _service.AddPasswordAsync(new Password
            {
                UserName = "user1",
                App = "app1",
                Category = "category1",
                EncryptedPassword = encryptedPassword
            });
            var result = await _service.GetPasswordByIdAsync(password.Id, true);
            Assert.NotNull(result);
            Assert.Equal(password.UserName, result.UserName);
            string decryptedPassword = Encoding.ASCII.GetString(Convert.FromBase64String(result.EncryptedPassword));
            Assert.Equal(originalPassword, decryptedPassword);
        }

        [Fact]
        public async Task UpdatePasswordAsync_ShouldUpdatePassword_IfExists()
        {
            var password = await _service.AddPasswordAsync(new Password { UserName = "user1", App = "app1", Category = "category1", EncryptedPassword = "myPassword" });
            var updatedPassword = new Password { UserName = "user1_updated", App = "app1_updated", Category = "category1_updated", EncryptedPassword = "myPassword_updated" };
            var result = await _service.UpdatePasswordAsync(password.Id, updatedPassword);
            Assert.NotNull(result);
            Assert.Equal("user1_updated", result.UserName);
        }

        [Fact]
        public async Task DeletePasswordAsync_ShouldReturnTrue_IfPasswordDeleted()
        {
            var password = await _service.AddPasswordAsync(new Password { UserName = "user1", App = "app1", Category = "category1", EncryptedPassword = "myPassword" });
            var result = await _service.DeletePasswordAsync(password.Id);
            Assert.True(result);
            Assert.Null(await _context.Passwords.FindAsync(password.Id));
        }

        [Fact]
        public async Task DeletePasswordAsync_ShouldReturnFalse_IfPasswordNotFound()
        {
            var result = await _service.DeletePasswordAsync(999);
            Assert.False(result);
        }
    }
}
