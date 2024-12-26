namespace UserService.BLL.DTO.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        required public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
