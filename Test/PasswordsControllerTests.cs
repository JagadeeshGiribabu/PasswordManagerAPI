using Microsoft.AspNetCore.Mvc;
using Moq;
using PasswordManagerApi.Controllers;
using PasswordManagerApi.Models;
using PasswordManagerApi.Services;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PasswordManagerApi.Tests
{
    public class PasswordsControllerTests
    {
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly PasswordsController _controller;

        public PasswordsControllerTests()
        {
            _mockPasswordService = new Mock<IPasswordService>();
            _controller = new PasswordsController(_mockPasswordService.Object);
        }

        [Fact]
        public async Task GetPasswords_ReturnsOkResult_WithListOfPasswords()
        {
            // Arrange
            var passwords = new List<Password>
            {
                new Password { Id = 1, App = "App1", UserName = "user1", EncryptedPassword = "encrypted1" },
                new Password { Id = 2, App = "App2", UserName = "user2", EncryptedPassword = "encrypted2" }
            };
            _mockPasswordService.Setup(service => service.GetPasswordsAsync())
                .ReturnsAsync(passwords);

            // Act
            var result = await _controller.GetPasswords();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnPasswords = Assert.IsType<List<Password>>(okResult.Value);
            Assert.Equal(2, returnPasswords.Count);
        }

        [Fact]
        public async Task GetPassword_ValidId_WithDecryption_ReturnsOkResult_WithDecryptedPassword()
        {
            // Arrange
            var password = new Password
            {
                Id = 1,
                App = "App1",
                UserName = "user1",
                EncryptedPassword = Convert.ToBase64String(Encoding.ASCII.GetBytes("plainTextPassword"))
            };
            _mockPasswordService.Setup(service => service.GetPasswordByIdAsync(1, true))
                .ReturnsAsync(password);

            // Act
            var result = await _controller.GetPassword(1, true); // Request with decryption

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnPassword = Assert.IsType<Password>(okResult.Value);
            Assert.Equal(1, returnPassword.Id);
            // Verify that the decrypted password is correctly set
            string decryptedPassword = Encoding.ASCII.GetString(Convert.FromBase64String(returnPassword.EncryptedPassword));
            Assert.Equal("plainTextPassword", decryptedPassword);
        }

        [Fact]
        public async Task GetPassword_ValidId_WithoutDecryption_ReturnsOkResult_WithEncryptedPassword()
        {
            // Arrange
            var password = new Password
            {
                Id = 1,
                App = "App1",
                UserName = "user1",
                EncryptedPassword = Convert.ToBase64String(Encoding.ASCII.GetBytes("plainTextPassword"))
            };
            _mockPasswordService.Setup(service => service.GetPasswordByIdAsync(1, false))
                .ReturnsAsync(password);

            // Act
            var result = await _controller.GetPassword(1, false); // Request without decryption

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnPassword = Assert.IsType<Password>(okResult.Value);
            Assert.Equal(1, returnPassword.Id);
            // Check that the encrypted password is returned
            Assert.Equal(password.EncryptedPassword, returnPassword.EncryptedPassword);
        }

        [Fact]
        public async Task GetPassword_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockPasswordService.Setup(service => service.GetPasswordByIdAsync(99, false))
                .ReturnsAsync((Password)null);

            // Act
            var result = await _controller.GetPassword(99, false); // Request without decryption

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task PostPassword_ValidPassword_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var password = new Password { Id = 1, App = "App1", UserName = "user1", EncryptedPassword = "encrypted1" };
            _mockPasswordService.Setup(service => service.AddPasswordAsync(It.IsAny<Password>()))
                .ReturnsAsync(password);

            // Act
            var result = await _controller.PostPassword(password);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnPassword = Assert.IsType<Password>(createdAtActionResult.Value);
            Assert.Equal(password.Id, returnPassword.Id);
        }

        [Fact]
        public async Task PutPassword_ValidPassword_ReturnsNoContent()
        {
            // Arrange
            var password = new Password { Id = 1, App = "App1", UserName = "user1", EncryptedPassword = "encrypted1" };
            _mockPasswordService.Setup(service => service.UpdatePasswordAsync(1, password))
                .ReturnsAsync(password);

            // Act
            var result = await _controller.PutPassword(1, password);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutPassword_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var password = new Password { Id = 1, App = "App1", UserName = "user1", EncryptedPassword = "encrypted1" };
            _mockPasswordService.Setup(service => service.UpdatePasswordAsync(99, password))
                .ReturnsAsync((Password)null);

            // Act
            var result = await _controller.PutPassword(99, password);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletePassword_ValidId_ReturnsNoContent()
        {
            // Arrange
            _mockPasswordService.Setup(service => service.DeletePasswordAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeletePassword(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePassword_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockPasswordService.Setup(service => service.DeletePasswordAsync(99))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeletePassword(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
