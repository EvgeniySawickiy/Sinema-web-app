using AutoMapper;
using TransportShop.DAL.Enums;
using UserService.BLL.DTO.Request;
using UserService.BLL.DTO.Response;
using UserService.DAL.Entities;

namespace UserService.BLL.Mappers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<User, UserResponse>();

            CreateMap<UpdateUserDto, User>();

            CreateMap<SignUpRequest, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<SignInRequest, Account>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
            CreateMap<SignUpRequest, Account>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Role.User))
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());
        }
    }
}
