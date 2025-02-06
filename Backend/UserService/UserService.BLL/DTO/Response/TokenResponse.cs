﻿namespace UserService.BLL.DTO.Response
{
    public class TokenResponse
    {
        required public Guid UserId { get; set; }
        required public string AccessToken { get; set; }
        required public string RefreshToken { get; set; }
    }
}
