using System.Reflection;
using BookingService.Application.Features.Bookings.Commands.Handlers;
using BookingService.Application.Features.Bookings.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateBookingCommandHandler).Assembly));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}