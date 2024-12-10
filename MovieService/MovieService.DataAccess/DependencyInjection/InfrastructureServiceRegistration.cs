using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieService.DataAccess.Interfaces;
using MovieService.DataAccess.Persistence;
using MovieService.DataAccess.Persistence.Repositories;

namespace MovieService.DataAccess.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IShowtimeRepository, ShowtimeRepository>();
            services.AddScoped<IHallRepository, HallRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddGrpc();

            return services;
        }
    }
}
