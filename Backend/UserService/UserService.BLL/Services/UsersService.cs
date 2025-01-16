using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using TransportShop.DAL.Enums;
using UserService.BLL.DTO.Request;
using UserService.BLL.DTO.Response;
using UserService.BLL.Exceptions;
using UserService.BLL.Interfaces;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class UsersService : IUserService
    {
        private readonly ILogger<UsersService> _logger;
        private IUserRepository userRepo;
        private IRefreshTokenRepository tokenRepo;
        private IAccountRepository accountRepo;
        private ITokenService tokenService;
        private IValidator<SignInRequest> signInValidator;
        private IValidator<SignUpRequest> signUpValidator;
        private IMapper mapper;

        public UsersService(
            IUserRepository userRepo,
            IRefreshTokenRepository tokenRepo,
            IAccountRepository accountRepo,
            ITokenService tokenService,
            IMapper mapper,
            IValidator<SignInRequest> signInValidator,
            IValidator<SignUpRequest> signUpValidator,
            ILogger<UsersService> logger)
        {
            this.userRepo = userRepo;
            this.tokenRepo = tokenRepo;
            this.accountRepo = accountRepo;
            this.tokenService = tokenService;
            this.signInValidator = signInValidator;
            this.signUpValidator = signUpValidator;
            this.mapper = mapper;
            this._logger = logger;
        }

        public async Task<TokenResponse> SignInAsync(SignInRequest request,
            CancellationToken cancellationToken = default)
        {
            await ValidateRequestAsync(signInValidator, request, cancellationToken);

            Account? account = await accountRepo.GetAccountByLoginAsync(request.Login, cancellationToken);
            if (account == null)
            {
                _logger.LogError("Error during user sign-in attempt {Login}", request.Login);
                throw new NotFoundException("User is not found");
            }

            if (account.PasswordHash != HashPassword(request.Password))
            {
                _logger.LogWarning("Incorrect password for user {Login}", request.Login);
                throw new BadRequestException("Wrong password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Login),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
            };

            _logger.LogInformation("Generating tokens for user {Login}", request.Login);

            string newAccessTokenString = tokenService.GenerateAccessToken(claims);
            string newRefreshTokenString = tokenService.GenerateRefreshToken();

            RefreshToken? refreshToken = await tokenRepo.GetByIdAsync(account.Id, cancellationToken);
            if (refreshToken != null)
            {
                refreshToken.Token = newRefreshTokenString;
                refreshToken.LifeTime = DateTime.UtcNow.AddDays(7);
                await tokenRepo.UpdateAsync(refreshToken);
            }
            else
            {
                RefreshToken newRefreshToken = new RefreshToken()
                {
                    Id = account.Id,
                    Token = newRefreshTokenString,
                    LifeTime = DateTime.Now.AddDays(7),
                };

                await tokenRepo.AddAsync(newRefreshToken);
            }

            return new TokenResponse
            {
                AccessToken = newAccessTokenString,
                RefreshToken = newRefreshTokenString,
            };
        }

        public async Task<TokenResponse> SignUpAsync(SignUpRequest request,
            CancellationToken cancellationToken = default)
        {
            await ValidateRequestAsync(signUpValidator, request, cancellationToken);

            if (await accountRepo.GetAccountByLoginAsync(request.Login, cancellationToken) != null)
            {
                _logger.LogWarning("Attempt to register with an existing login {Login}", request.Login);
                throw new BadRequestException("Login is already in use");
            }

            _logger.LogInformation("Creating a new user and account for {Login}", request.Login);

            User user = mapper.Map<User>(request);
            user.Id = Guid.NewGuid();

            await userRepo.AddAsync(user);

            Account account = mapper.Map<Account>(request);
            account.Id = Guid.NewGuid();
            account.Role = Role.User;
            account.User = user;
            account.PasswordHash = HashPassword(request.Password);

            await accountRepo.AddAsync(account, cancellationToken);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Login),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
            };

            string newAccessTokenString = tokenService.GenerateAccessToken(claims);
            string newRefreshTokenString = tokenService.GenerateRefreshToken();

            RefreshToken newRefreshToken = new RefreshToken()
            {
                Id = account.Id,
                Token = newRefreshTokenString,
                LifeTime = DateTime.UtcNow.AddDays(7),
            };

            await tokenRepo.AddAsync(newRefreshToken);

            return new TokenResponse
            {
                AccessToken = newAccessTokenString,
                RefreshToken = newRefreshTokenString,
            };
        }

        public async Task<TokenResponse> RefreshTokenAsync(TokenRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Refreshing token");
            string requestAccessToken = request.AccessToken;
            string requestRefreshToken = request.RefreshToken;

            var principal = tokenService.GetPrincipalFromExpiredToken(requestAccessToken);
            string login = principal.Identity.Name;

            Account? account = await accountRepo.GetAccountByLoginAsync(login, cancellationToken);
            if (account == null)
            {
                throw new NotFoundException("User is not found");
            }

            RefreshToken refreshToken = await tokenRepo.GetByIdAsync(account.Id, cancellationToken);
            if (refreshToken.Token != requestRefreshToken)
            {
                throw new BadRequestException("Incorrect refresh token");
            }

            if (refreshToken.LifeTime <= DateTime.Now)
            {
                throw new BadRequestException("The token has expired, please log in again");
            }

            string newAccessToken = tokenService.GenerateAccessToken(principal.Claims);
            string newRefreshToken = tokenService.GenerateRefreshToken();

            refreshToken.Token = newRefreshToken;
            await tokenRepo.UpdateAsync(refreshToken);

            return new TokenResponse()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };
        }

        public async Task<Guid> GetMyIdByJwtAsync(ClaimsPrincipal principal,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving user ID from JWT");

            string? login = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (login == null)
            {
                _logger.LogWarning("User name not found in token");
                throw new NotFoundException("User is not found");
            }

            Account? account = await accountRepo.GetAccountByLoginAsync(login, cancellationToken);
            if (account == null)
            {
                throw new NotFoundException("Account is not found");
            }

            return account.Id;
        }

        public async Task<UserResponse> GetMyProfileByJwtAsync(ClaimsPrincipal principal,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Requesting user profile");
            string? login = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (login == null)
            {
                throw new NotFoundException("User is not found");
            }

            Account? account = await accountRepo.GetAccountByLoginAsync(login, cancellationToken);
            if (account == null)
            {
                throw new NotFoundException("Account is not found");
            }

            User? user = await userRepo.GetByIdAsync(account.Id, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("Profile is not found");
            }

            UserResponse response = mapper.Map<UserResponse>(user);

            return response;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<User> users = await userRepo.GetAllAsync(cancellationToken);
            if (users == null || !users.Any())
            {
                _logger.LogWarning("Users not found");
                throw new NotFoundException("Users not found");
            }

            IEnumerable<UserResponse> response = mapper.Map<IEnumerable<UserResponse>>(users);
            return response;
        }

        public async Task<UserResponse> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            User user = await userRepo.GetByIdAsync(id, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User with id {userId} not found", id);
                throw new NotFoundException("User is not found");
            }

            UserResponse response = mapper.Map<UserResponse>(user);
            return response;
        }

        public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Account account = await accountRepo.GetByIdAsync(id, cancellationToken);
            if (account == null)
            {
                _logger.LogWarning("User with id {userId} not found", id);
                throw new NotFoundException("Account is not found");
            }

            await accountRepo.DeleteAsync(account, cancellationToken);
        }

        private async Task ValidateRequestAsync<T>(IValidator<T> validator, T request,
            CancellationToken cancellationToken)
        {
            _logger.LogDebug("Validating request of type {Type}", typeof(T).Name);
            ValidationResult result = await validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                throw new BadRequestException(errors);
            }
        }

        private string HashPassword(string password)
        {
            _logger.LogDebug("Hashing password");
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}