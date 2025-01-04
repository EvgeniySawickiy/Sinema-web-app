using System.Text;
using Microsoft.IdentityModel.Tokens;
using MovieService.Application.DependencyInjection;
using MovieService.DataAccess.DependencyInjection;
using MovieService.DataAccess.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    var configuration = builder.Configuration.GetSection("AppSettings");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["JwtIssuer"],
        ValidAudience = configuration["JwtAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(configuration["JwtSecretKey"])),
    };
    options.SaveToken = true;
});
var app = builder.Build();

app.MapGrpcService<MovieServiceGrpc>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
