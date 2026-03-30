using QuantityMeasurementAppModelLayer.Models;

namespace QuantityMeasurementAppRepositoryLayer.Interface
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
}
