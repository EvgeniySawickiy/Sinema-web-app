using System.Text.Json;
using AutoMapper;
using MovieService.Application.DTO.Hall;
using MovieService.Application.DTO.Hall;
using MovieService.Application.DTO.Movie;
using MovieService.Application.DTO.Showtime;
using MovieService.Application.Services;
using MovieService.Application.UseCases.Showtimes.Commands;
using MovieService.Core.Entities;

namespace MovieService.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hall, HallDto>();
            CreateMap<UpdateShowtimeCommand, Showtime>();

            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Genres, opt =>
                    opt.MapFrom(src => src.MovieGenres.Select(mg => mg.Genre.Name).ToList()));

            CreateMap<Showtime, ShowtimeDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Movie.Title))
                .ForMember(dest => dest.MovieGenres, opt =>
                    opt.MapFrom(src => src.Movie.MovieGenres.Select(mg => mg.Genre.Name).ToList()))
                .ForMember(dest => dest.HallName, opt => opt.MapFrom(src => src.Hall.Name));
        }
    }
}