namespace UserService.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public DateTime? EmailConfirmationTokenExpiresAt { get; set; }

        public Account Account { get; set; }
    }
}