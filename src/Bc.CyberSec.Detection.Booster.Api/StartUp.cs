using AspNetCore.Authentication.ApiKey;
using Bc.CyberSec.Detection.Booster.Api.Core.Infrastructure;
using Bc.CyberSec.Detection.Booster.Api.Core.Infrastructure.Auth;

namespace Bc.CyberSec.Detection.Booster.Api;

public class StartUp
{
    public static void InitApiServices(IConfiguration configuration, IServiceCollection services)
    {
        services.AddTransient<ApiKeyProvider>(x => new ApiKeyProvider(configuration));
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMemoryCache();
        services.AddAuthentication()
            .AddApiKeyInHeader<ApiKeyProvider>(options =>
            {
                options.Realm = "syslog-ng.configurator.api";
                options.KeyName = "Key";
            });
        services.AddAuthorization();
    }

    public static void Configure(IConfiguration configuration, IServiceCollection services)
    {
        InitApiServices(configuration, services);
        services.AddDetectionBoosterCore(configuration);
        services.AddMongoDb(Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING") ?? configuration["MONGO_CONNECTION_STRING"]);
    }
}