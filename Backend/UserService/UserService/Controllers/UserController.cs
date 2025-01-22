using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportShop.DAL.Enums;
using UserService.API.Extensions;
using UserService.BLL.DTO.Request;
using UserService.BLL.Interfaces;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ITokenService tokenService, ILogger<UserController> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User login request with username {Login}", request.Login);

            var response = await _userService.SignInAsync(request, cancellationToken);

            _logger.LogInformation("User {Login} successfully signed in", request.Login);

            return Ok(response);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request, CancellationToken cancellationToken)
        {
            var response = await _userService.SignUpAsync(request, cancellationToken);
            var userId =
                await _userService.GetMyIdByJwtAsync(_tokenService.GetPrincipalFromExpiredToken(response.AccessToken));
            _logger.LogInformation("User {Login} successfully registered with ID {UserId}", request.Login, userId);

            return CreatedAtAction(nameof(GetUserById), new { id = userId }, response);
        }

        [HttpPost]
        [Route("token/refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Token refresh request");
            var response = await _userService.RefreshTokenAsync(request, cancellationToken);
            _logger.LogInformation("Token successfully refreshed");

            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request to retrieve the current user's profile");

            var response = await _userService.GetMyProfileByJwtAsync(HttpContext.User);

            _logger.LogInformation("Current user's profile successfully retrieved");

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request to retrieve all users");

            var users = await _userService.GetAllUsersAsync(cancellationToken);

            _logger.LogInformation("List of all users successfully retrieved");

            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = $"{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request to retrieve user with ID {UserId}", id);

            var user = await _userService.GetUserByIdAsync(id, cancellationToken);

            _logger.LogInformation("User with ID {UserId} successfully retrieved", id);

            return Ok(user);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request to delete user with ID {UserId}", id);

            await _userService.DeleteUserAsync(id, cancellationToken);

            _logger.LogInformation("User with ID {UserId} successfully deleted", id);

            return NoContent();
        }

        [HttpPost("{userId:guid}/send-confirmation-email")]
        public async Task<IActionResult> SendConfirmationEmail(Guid userId)
        {
            _logger.LogInformation("Received request to send confirmation email to user with ID {UserId}", userId);

            await _userService.SendConfirmationEmail(userId);

            _logger.LogInformation("Confirmation email sent to user with ID {UserId}", userId);

            return Ok(new { Message = "Confirmation email sent." });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            _logger.LogInformation("Received request to confirm email with token {Token}", token);

            await _userService.ConfirmEmail(token);

            _logger.LogInformation("Email confirmed successfully for token {Token}", token);

            return Ok(new { Message = "Email confirmed successfully." });
        }

        [HttpPost("password-reset/request")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequest request)
        {
            _logger.LogInformation("Received request to reset password for email {Email}", request.Email);

            await _userService.RequestPasswordResetAsync(request.Email);

            _logger.LogInformation("Password reset email sent successfully to {Email}", request.Email);

            return Ok(new { Message = "Password reset email sent successfully." });
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetConfirmRequest request)
        {
            _logger.LogInformation("Received request to reset password with token {Token}", request.Token);

            await _userService.ResetPasswordAsync(request.Token, request.NewPassword);

            _logger.LogInformation("Password successfully reset for token {Token}", request.Token);

            return Ok(new { Message = "Password has been reset successfully." });
        }
    }
}