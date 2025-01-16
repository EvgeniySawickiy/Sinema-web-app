using Serilog;
namespace MovieService.Extention;

public static class AddLoggingExtension
{
    public static IHostBuilder AddLogs(this IHostBuilder @this, IConfiguration configuration)
    {
        if (configuration["Logstash:Uri"] is not string logstashUri)
        {
            throw new KeyNotFoundException("There is no Logstash:Uri in configuration");
        }

        @this.UseSerilog((context, config) =>
        {
            config.WriteTo.Http(logstashUri, queueLimitBytes: null);
        });

        return @this;
    }
}