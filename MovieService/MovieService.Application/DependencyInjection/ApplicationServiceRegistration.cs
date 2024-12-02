using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieService.Application.Mappers;
using MovieService.Application.UseCases.Movies.Commands;
using MovieService.Application.Validators;

namespace MovieService.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly));
            services.AddValidatorsFromAssemblyContaining<CreateMovieValidator>();

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
