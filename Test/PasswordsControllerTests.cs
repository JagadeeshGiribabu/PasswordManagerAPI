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
            var passwords = new List<Password>
            {
                new Password { Id = 1, App = "App1", UserName = "user1", EncryptedPassword = "encrypted1" },
                new Password { Id = 2, App = "App2", UserName = "user2", EncryptedPassword = "encrypted2" }
            };
            _mockPasswordService.Setup(service => service.GetPasswordsAsync())
                .ReturnsAsync(passwords);
            var result = await _controller.GetPasswords();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnPasswords = Assert.IsType<List<Password>>(okResult.Value);
            Assert.Equal(2, returnPasswords.Count);
        }

        [Fact]
        public async Task GetPassword_ValidId_WithDecryption_ReturnsOkResult_WithDecryptedPassword()
        {
            var password = new Password
            {
                Id = 1,
                App = "App1",
                UserName = "user1",
                EncryptedPassword = Convert.ToBase64String(Encoding.ASCII.GetBytes("plainTextPassword"))
            };
            _mockPasswordService.Setup(service => service.GetPasswordByIdAsync(1, true))
                .ReturnsAsync(password);
            var result = await _controller.GetPassword(1, true);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnPassword = Assert.IsType<Password>(okResult.Value);
            Assert.Equal(1, returnPassword.Id);
            string decryptedPassword = Encoding.ASCII.GetString(Convert.FromBase64String(returnPassword.EncryptedPassword));
            Assert.Equal("plainTextPassword", decryptedPassword);
        }

        [Fact]
        public async Task GetPassword_ValidId_WithoutDecryption_ReturnsOkResult_WithEncryptedPassword()
        {
            var password = new Password
            {
                Id = 1,
                App = "App1",
                UserName = "user1",
                EncryptedPassword = Convert.ToBase64String(Encoding.ASCII.GetBytes("plainTextPassword"))
            };
            _mockPasswordService.Setup(service => service.GetPasswordByIdAsync(1, false))
                .ReturnsAsync(password);
            var result = await _controller.GetPassword(1, false);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnPassword = Assert.IsType<Password>(okResult.Value);
            Assert.Equal(1, returnPassword.Id);
            Assert.Equal(password.EncryptedPassword, returnPassword.EncryptedPassword);
        }

        [Fact]
        public async Task GetPassword_InvalidId_ReturnsNotFound()
        {
            _mockPasswordService.Setup(service => service.GetPasswordByIdAsync(99, false))
                .ReturnsAsync((Password)null);
            var result = await _controller.GetPassword(99, false);
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task PostPassword_ValidPassword_ReturnsCreatedAtActionResult()
        {
            var password = new Password { Id = 1, App = "App1", UserName = "user1", EncryptedPassword = "encrypted1" };
            _mockPasswordService.Setup(service => service.AddPasswordAsync(It.IsAny<Password>()))
                .ReturnsAsync(password);
            var result = await _controller.PostPassword(password);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnPassword = Assert.IsType<Password>(createdAtActionResult.Value);
            Assert.Equal(password.Id, returnPassword.Id);
        }

        [Fact]
        public async Task PutPassword_ValidPassword_ReturnsNoContent()
        {
            var password = new Password { Id = 1, App = "App1", UserName = "user1", EncryptedPassword = "encrypted1" };
            _mockPasswordService.Setup(service => service.UpdatePasswordAsync(1, password))
                .ReturnsAsync(password);
            var result = await _controller.PutPassword(1, password);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutPassword_InvalidId_ReturnsNotFound()
        {
            var password = new Password { Id = 1, App = "App1", UserName = "user1", EncryptedPassword = "encrypted1" };
            _mockPasswordService.Setup(service => service.UpdatePasswordAsync(99, password))
                .ReturnsAsync((Password)null);
            var result = await _controller.PutPassword(99, password);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletePassword_ValidId_ReturnsNoContent()
        {
            _mockPasswordService.Setup(service => service.DeletePasswordAsync(1))
                .ReturnsAsync(true);
            var result = await _controller.DeletePassword(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePassword_InvalidId_ReturnsNotFound()
        {
            _mockPasswordService.Setup(service => service.DeletePasswordAsync(99))
                .ReturnsAsync(false);
            var result = await _controller.DeletePassword(99);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
