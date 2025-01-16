using System.Text;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotificationService.Protos;
using UserService.BLL.DTO.Validators;
using UserService.BLL.Interfaces;
using UserService.BLL.Services;
using UserService.DAL.EF;
using UserService.DAL.Interfaces;
using UserService.DAL.Repositories;

namespace UserService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpc();

        services.AddGrpcClient<Notification.NotificationClient>(o =>
        {
            o.Address = new Uri(configuration["Grpc:NotificationServiceUrl"]);
        });

        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UsersService>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddValidatorsFromAssemblyContaining<SignInRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SignUpRequestValidator>();
        services.AddSingleton(new EmailTemplateLoader(Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Email")));
        services.AddControllers();

        services.AddAuthentication("Bearer").AddJwtBearer(options =>
        {
            var appSettings = configuration.GetSection("AppSettings");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = appSettings["JwtIssuer"],
                ValidAudience = appSettings["JwtAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings["JwtSecretKey"])),
            };
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Please enter token",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    new string[] { }
                },
            });
        });

        return services;
    }
}