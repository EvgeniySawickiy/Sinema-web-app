namespace UserService.BLL.DTO.Request
{
    public class SignInRequest
    {
        required public string Login { get; set; }
        required public string Password { get; set; }
    }
}
