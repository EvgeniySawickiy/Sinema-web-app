namespace UserService.BLL.DTO.Request
{
    public class PasswordResetConfirmRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
