using Grpc.Core;
using UserService.DAL.Interfaces;
using UserService.Grpc;
using static UserService.Grpc.UserService;

namespace UserService.DAL.External_Services
{
    public class UserServiceGrpc : UserServiceBase
    {
        private readonly IUserRepository _userRepository;

        public UserServiceGrpc(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            var user = await _userRepository.GetByIdAsync(Guid.Parse(request.UserId));

            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"User {request.UserId} not found"));
            }

            return new GetUserByIdResponse
            {
                UserId = user.Id.ToString(),
                Email = user.Email.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
            };
        }

        public override async Task<GetUsersResponse> GetUsers(EmptyRequest request, ServerCallContext context)
        {
            var response = new GetUsersResponse();

            var users = await _userRepository.GetAllAsync();

            foreach (var user in users)
            {
                response.Users.Add(new GetUserByIdResponse
                {
                    UserId = user.Id.ToString(),
                    Email = user.Email.ToString(),
                    Name = user.Name,
                    Surname = user.Surname,
                    PhoneNumber = user.PhoneNumber,
                });
            }

            return response;
        }
    }
}
