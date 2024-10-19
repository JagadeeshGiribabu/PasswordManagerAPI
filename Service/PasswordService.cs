using Microsoft.EntityFrameworkCore;
using PasswordManagerApi.Data;
using PasswordManagerApi.Models;
using System.Text;

namespace PasswordManagerApi.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly DataContext _context;

        public PasswordService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Password>> GetPasswordsAsync()
        {
            return await _context.Passwords.ToListAsync();
        }

        public async Task<Password?> GetPasswordByIdAsync(int id, bool includeDecrypted)
        {
            var password = await _context.Passwords.FindAsync(id);
            if (password == null)
            {
                return null;
            }

            if (includeDecrypted)
            {
                byte[] decryptedBytes = Convert.FromBase64String(password.EncryptedPassword);
                string decryptedPassword = Encoding.ASCII.GetString(decryptedBytes);
                password.EncryptedPassword = decryptedPassword; 
            }

            return password;

        }

        public async Task<Password> AddPasswordAsync(Password password)
        {
            password.EncryptedPassword = Convert.ToBase64String(Encoding.ASCII.GetBytes(password.EncryptedPassword));
            _context.Passwords.Add(password);
            await _context.SaveChangesAsync();
            return password;
        }

        public async Task<Password?> UpdatePasswordAsync(int id, Password password)
        {
            var existingPassword = await _context.Passwords.FindAsync(id);
            if (existingPassword == null)
            {
                return null;
            }

            existingPassword.Category = password.Category;
            existingPassword.App = password.App;
            existingPassword.UserName = password.UserName;
            existingPassword.EncryptedPassword = Convert.ToBase64String(Encoding.ASCII.GetBytes(password.EncryptedPassword));

            _context.Entry(existingPassword).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingPassword;
        }

        public async Task<bool> DeletePasswordAsync(int id)
        {
            var password = await _context.Passwords.FindAsync(id);
            if (password == null)
            {
                return false;
            }

            _context.Passwords.Remove(password);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
