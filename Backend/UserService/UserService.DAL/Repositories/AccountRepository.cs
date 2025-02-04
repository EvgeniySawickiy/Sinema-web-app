using Microsoft.EntityFrameworkCore;
using UserService.DAL.EF;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class AccountRepository(DataContext context) : Repository<Account>(context), IAccountRepository
    {
        private readonly DataContext _context = context;

        public async Task<Account?> GetAccountByLoginAsync(string login, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Login == login, cancellationToken);
        }

        public async Task<Account> GetAccountByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == userId, cancellationToken);
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _context.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.User.Email == email);
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task<Account?> GetByResetTokenAsync(string token)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.PasswordResetToken == token);
        }
    }
}
