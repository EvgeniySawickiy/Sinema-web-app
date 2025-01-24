namespace UserService.BLL.DTO.Response;

public class SignUpResponse
{
    required public Guid UserId { get; set; }
    required public string AccessToken { get; set; }
    required public string RefreshToken { get; set; }
}