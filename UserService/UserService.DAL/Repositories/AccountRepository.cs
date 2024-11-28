using Microsoft.EntityFrameworkCore;
using UserService.DAL.EF;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    internal class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<Account?> GetAccountByLoginAsync(string login, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Login == login, cancellationToken);
        }

        public async Task<Account> GetAccountByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Id == userId, cancellationToken);
        }
    }
}
