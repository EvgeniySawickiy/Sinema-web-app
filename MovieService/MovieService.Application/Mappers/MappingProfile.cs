using AutoMapper;
using MovieService.Application.DTO.Hall;
using MovieService.Application.DTO.Movie;
using MovieService.Application.DTO.Showtime;
using MovieService.Core.Entities;
using MovieService.Core.Enums;

namespace MovieService.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hall, HallDto>();
            CreateMap<CreateHallDto, Hall>();
            CreateMap<UpdateHallDto, Hall>();

            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.ToString()));
            CreateMap<CreateMovieDto, Movie>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => Enum.Parse<Genre>(src.Genre, true)));
            CreateMap<UpdateMovieDto, Movie>();

            CreateMap<Showtime, ShowtimeDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Movie.Title))
                .ForMember(dest => dest.HallName, opt => opt.MapFrom(src => src.Hall.Name));
            CreateMap<CreateShowtimeDto, Showtime>();
            CreateMap<UpdateShowtimeDto, Showtime>();
        }
    }
}
