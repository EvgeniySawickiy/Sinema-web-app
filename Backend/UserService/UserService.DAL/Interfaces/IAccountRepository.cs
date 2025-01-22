using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        public Task<Account> GetAccountByUserAsync(Guid userId, CancellationToken cancellationToken = default);
        public Task<Account?> GetAccountByLoginAsync(string login, CancellationToken cancellationToken = default);
        public Task<Account?> GetByEmailAsync(string email);
        public Task<Account?> GetByResetTokenAsync(string token);
        public Task UpdateAsync(Account account);
    }
}
