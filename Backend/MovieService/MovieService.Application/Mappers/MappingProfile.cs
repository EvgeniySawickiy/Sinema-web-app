using System.Text.Json;
using AutoMapper;
using MovieService.Application.DTO.Hall;
using MovieService.Application.DTO.Hall;
using MovieService.Application.DTO.Movie;
using MovieService.Application.DTO.Showtime;
using MovieService.Application.Services;
using MovieService.Core.Entities;

namespace MovieService.Application.Mappers
{
    public class MappingProfile : Profile
    {
        private readonly SeatLayoutSerializer _seatLayoutSerializer;

        public MappingProfile(SeatLayoutSerializer seatLayoutSerializer)
        {
            _seatLayoutSerializer = seatLayoutSerializer;

            CreateMap<Hall, HallDto>()
                .ForMember(dest => dest.SeatLayout, opt =>
                    opt.MapFrom(src => DeserializeSeatLayout(src.SeatLayoutJson)));

            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Genres, opt =>
                    opt.MapFrom(src => src.MovieGenres.Select(mg => mg.Genre.Name).ToList()));

            CreateMap<Showtime, ShowtimeDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Movie.Title))
                .ForMember(dest => dest.MovieGenres, opt =>
                    opt.MapFrom(src => src.Movie.MovieGenres.Select(mg => mg.Genre.Name).ToList()))
                .ForMember(dest => dest.HallName, opt => opt.MapFrom(src => src.Hall.Name));
        }

        private List<RowDto>? DeserializeSeatLayout(string? seatLayoutJson)
        {
            if (string.IsNullOrEmpty(seatLayoutJson))
            {
                return null;
            }

            var seatLayout = _seatLayoutSerializer.Deserialize(seatLayoutJson);
            return seatLayout.Rows.Select(row => new RowDto
            {
                RowNumber = row.RowNumber,
                Seats = row.Seats,
            }).ToList();
        }
    }
}