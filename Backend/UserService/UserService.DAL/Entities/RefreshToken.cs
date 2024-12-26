namespace UserService.DAL.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string? Token { get; set; }
        public DateTime? LifeTime { get; set; }

        public Account? Account { get; set; }
    }
}
