using Application.Common.Caching.Interfaces;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Database;
using Infrastructure.Caching;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("ApplicationDbContext"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
        );

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.ConfigRepositories();
        return services;
    }

    private static IServiceCollection ConfigRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDistributedMemoryCache();
        services.AddTransient<IRedisCacheService, RedisCacheService>();

        var redisEndPoint = configuration["Redis:EndPointHost"];

#if DEBUG
        redisEndPoint = configuration["Redis:EndPointHostDebug"];
#endif

        var redisConfigure = $"{redisEndPoint}," +
            $"channelPrefix={configuration["Redis:ChannelPrefix"]}," +
            $"defaultDatabase={configuration["Redis:DefaultDatabase"]}";

        services.AddStackExchangeRedisCache(cfg =>
        {
            cfg.Configuration = redisConfigure;
        });

        return services;
    }

    private static IServiceCollection ConfigRepositories(this IServiceCollection services)
    {
        services.AddTransient<IDateTime, DateTimeService>();
        return services;
    }

}
