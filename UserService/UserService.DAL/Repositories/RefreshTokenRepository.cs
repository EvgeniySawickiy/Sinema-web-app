using Microsoft.EntityFrameworkCore;
using UserService.DAL.EF;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DataContext context)
            : base(context)
        {
        }
    }
}
