using Microsoft.EntityFrameworkCore;
using UserService.DAL.EF;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class UserRepository(DataContext context) : Repository<User>(context), IUserRepository
    {
        private readonly DataContext _context = context;

        public async Task<User?> GetByConfirmationTokenAsync(string token)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.EmailConfirmationToken == token);
        }
    }
}