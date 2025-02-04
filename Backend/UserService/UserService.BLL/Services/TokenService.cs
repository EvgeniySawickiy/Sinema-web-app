using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using UserService.BLL.Interfaces;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, IRefreshTokenRepository tokenRepo,
            ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            _logger.LogInformation("AccessToken generation started");

            var secretKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:JwtSecretKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration["AppSettings:JwtIssuer"],
                audience: _configuration["AppSettings:JwtAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    double.Parse(_configuration["AppSettings:JwtAccessTokenExpirationMinutes"])),
                signingCredentials: signinCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            _logger.LogInformation("AccessToken successfully generated. Valid until {Expiration}",
                tokeOptions.ValidTo);

            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            _logger.LogInformation("RefreshToken generation started");

            var randomNumber = new byte[32];
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            _logger.LogInformation("RefreshToken successfully generated");

            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            _logger.LogInformation("Retrieving ClaimsPrincipal from expired token");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:JwtSecretKey"])),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogWarning("Invalid token encountered when attempting to retrieve ClaimsPrincipal");
                throw new SecurityTokenException("Invalid token");
            }

            _logger.LogInformation("ClaimsPrincipal successfully retrieved from token");

            return principal;
        }
    }
}