using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using NotificationService.Core.Entities;
using NotificationService.Infrastructure.Services;
using UserService.Grpc;
using static UserService.Grpc.UserService;

namespace NotificationService.Infrastructure.Protos;

public class UserServiceGrpcClient : IUserService
{
    private readonly UserServiceClient _client;

    public UserServiceGrpcClient(IConfiguration configuration)
    {
        var grpcChannel = GrpcChannel.ForAddress(configuration["Grpc:UserServiceUrl"]);
        _client = new UserServiceClient(grpcChannel);
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        try
        {
            var request = new GetUserByIdRequest { UserId = userId.ToString() };
            var response = await _client.GetUserByIdAsync(request);

            return new User
            {
                Id = Guid.Parse(response.UserId),
                Email = response.Email,
                PhoneNumber = response.PhoneNumber,
                Name = response.Name,
                Surname = response.Surname,
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching user data: {ex.Message}");
        }
    }
}