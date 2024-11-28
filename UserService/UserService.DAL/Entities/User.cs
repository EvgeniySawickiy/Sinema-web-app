namespace UserService.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        required public string Login { get; set; }
        required public string PasswordHash { get; set; }
        required public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
