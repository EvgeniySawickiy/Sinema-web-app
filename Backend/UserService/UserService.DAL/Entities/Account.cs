using TransportShop.DAL.Enums;

namespace UserService.DAL.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        required public string Login { get; set; }
        required public string PasswordHash { get; set; }
        public Role Role { get; set; }

        public User? User { get; set; }
        public RefreshToken? RefreshToken { get; set; }
    }
}
