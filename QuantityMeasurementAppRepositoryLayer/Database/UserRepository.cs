using Microsoft.EntityFrameworkCore;
using QuantityMeasurementAppModelLayer.Models;
using QuantityMeasurementAppRepositoryLayer.Data;
using QuantityMeasurementAppRepositoryLayer.Interface;

namespace QuantityMeasurementAppRepositoryLayer.Database
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) => _db = db;

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
