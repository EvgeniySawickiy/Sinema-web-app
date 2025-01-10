using AutoMapper;
using BookingService.Application.DTO;
using BookingService.Core.Entities;

namespace BookingService.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Booking, BookingDTO>().ReverseMap()
                .ForMember(dest => dest.Seats, opt => opt.MapFrom(src => src.Seats));

            CreateMap<BookingResponseDTO, Booking>().ReverseMap();

            CreateMap<Seat, SeatDTO>().ReverseMap();
        }
    }
}
