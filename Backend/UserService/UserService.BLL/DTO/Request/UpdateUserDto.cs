namespace UserService.BLL.DTO.Request;

public class UpdateUserDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}