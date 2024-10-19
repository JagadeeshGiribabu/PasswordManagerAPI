using PasswordManagerApi.Models;

namespace PasswordManagerApi.Services
{
    public interface IPasswordService
    {
        Task<IEnumerable<Password>> GetPasswordsAsync();
        Task<Password?> GetPasswordByIdAsync(int id, bool includeDecrypted);
        Task<Password> AddPasswordAsync(Password password);
        Task<Password?> UpdatePasswordAsync(int id, Password password);
        Task<bool> DeletePasswordAsync(int id);
    }
}