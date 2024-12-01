using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieService.Application.Mappers;

namespace MovieService.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateMovieCommand).Assembly);
            services.AddValidatorsFromAssemblyContaining<CreateMovieValidator>();

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
