using Microsoft.AspNetCore.Mvc;
using PasswordManagerApi.Models;
using PasswordManagerApi.Services;

namespace PasswordManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        private readonly IPasswordService _passwordService;

        public PasswordsController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpPost]
        public async Task<ActionResult<Password>> PostPassword(Password password)
        {
            var createdPassword = await _passwordService.AddPasswordAsync(password);
            return CreatedAtAction(nameof(GetPassword), new { id = createdPassword.Id }, createdPassword);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Password>>> GetPasswords()
        {
            var passwords = await _passwordService.GetPasswordsAsync();
            return Ok(passwords);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Password>> GetPassword(int id, bool includeDecrypted = false)
        {
            var password = await _passwordService.GetPasswordByIdAsync(id, includeDecrypted);

            if (password == null)
            {
                return NotFound();
            }

            return Ok(password);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPassword(int id, Password password)
        {
            var updatedPassword = await _passwordService.UpdatePasswordAsync(id, password);
            if (updatedPassword == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassword(int id)
        {
            var isDeleted = await _passwordService.DeletePasswordAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
