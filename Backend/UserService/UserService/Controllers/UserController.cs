using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportShop.DAL.Enums;
using UserService.BLL.DTO.Request;
using UserService.BLL.Interfaces;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("/auth/signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request, CancellationToken cancellationToken)
        {
            var response = await _userService.SignInAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [Route("/auth/signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request, CancellationToken cancellationToken)
        {
            var response = await _userService.SignUpAsync(request, cancellationToken);
            var userId = await _userService.GetMyIdByJwtAsync(_tokenService.GetPrincipalFromExpiredToken(response.AccessToken));
            return CreatedAtAction(nameof(GetUserById), new { id = userId }, response);
        }

        [HttpPost]
        [Route("/auth/refreshtoken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request, CancellationToken cancellationToken)
        {
            var response = await _userService.RefreshTokenAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
        {
            var response = await _userService.GetMyProfileByJwtAsync(HttpContext.User);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllUsersAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = $"{nameof(Role.Manager)},{nameof(Role.Admin)}")]
        public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(id, cancellationToken);
            return Ok(user);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
