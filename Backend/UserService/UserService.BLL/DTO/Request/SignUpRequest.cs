namespace UserService.BLL.DTO.Request
{
    public class SignUpRequest
    {
        required public string Login { get; set; }
        required public string Password { get; set; }
        required public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
