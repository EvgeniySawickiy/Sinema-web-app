using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByConfirmationTokenAsync(string token);
    }
}
