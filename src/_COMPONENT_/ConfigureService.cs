using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _COMPONENT_.Helpers.HealthCheck;
using _COMPONENT_.Helpers.NSwag;
using _COMPONENT_.Middleware;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace _COMPONENT_;

public static class ConfigureService
{
    public static IServiceCollection AddComponentService(this IServiceCollection services,
    IConfiguration configuration, string environmentName)
    {
        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddCheck<AliveHealthCheck>("alive", tags: new[] { "alive" });

        services.AddCors(options =>
        {
            options.AddPolicy("CORS", policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            });
        });

        services.AddOpenApiDocument(config =>
        {
            config.Title = "_COMPONENT_ API";
            config.Version = "v1";
            config.Description = $"_COMPONENT_ API {environmentName}";
            config.OperationProcessors.Add(new AddRequiredHeaderParameter());
        });

        //Middleware
        services.AddTransient<RequestHeaderMiddleware>();

        return services;
    }
}
