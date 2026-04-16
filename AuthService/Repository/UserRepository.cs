using Microsoft.EntityFrameworkCore;
using Shared.Models;
using AuthService.Data;

namespace AuthService.Repository
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<ApplicationUser?> GetByGoogleIdAsync(string googleId);
        Task<ApplicationUser?> GetByIdAsync(int id);
        Task<ApplicationUser>  CreateAsync(ApplicationUser user);
        Task                   UpdateAsync(ApplicationUser user);
        Task<bool>             ExistsByEmailAsync(string email);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _db;

        public UserRepository(AuthDbContext db) => _db = db;

        public Task<ApplicationUser?> GetByEmailAsync(string email)
            => _db.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());

        public Task<ApplicationUser?> GetByGoogleIdAsync(string googleId)
            => _db.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId);

        public Task<ApplicationUser?> GetByIdAsync(int id)
            => _db.Users.FindAsync(id).AsTask()!;

        public async Task<ApplicationUser> CreateAsync(ApplicationUser user)
        {
            user.Email = user.Email.ToLowerInvariant();
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public Task<bool> ExistsByEmailAsync(string email)
            => _db.Users.AnyAsync(u => u.Email == email.ToLowerInvariant());
    }
}
