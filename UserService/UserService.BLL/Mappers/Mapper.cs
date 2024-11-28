using AutoMapper;
using UserService.BLL.DTO.Request;
using UserService.BLL.DTO.Response;
using UserService.DAL.Entities;

namespace UserService.BLL.Mappers
{
    internal class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<User, UserResponse>();

            CreateMap<SignUpRequest, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<SignInRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        }
    }
}
