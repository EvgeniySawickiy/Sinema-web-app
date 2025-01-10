namespace UserService.BLL.DTO.Request
{
    public class TokenRequest
    {
        required public string AccessToken { get; set; }
        required public string RefreshToken { get; set; }
    }
}
