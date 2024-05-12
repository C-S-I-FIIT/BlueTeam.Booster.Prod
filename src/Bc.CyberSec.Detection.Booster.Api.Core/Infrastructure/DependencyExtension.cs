using Bc.CyberSec.Detection.Booster.Api.Core.Application.Kibana;
using Bc.CyberSec.Detection.Booster.Api.Core.Application.Serialization;
using Bc.CyberSec.Detection.Booster.Api.Core.Application.SyslogNg;
using Bc.CyberSec.Detection.Booster.Api.Core.Application;
using Bc.CyberSec.Detection.Booster.Api.Core.Data;
using Bc.CyberSec.Detection.Booster.Api.Core.Model.UseCase;
using Bc.SyslogNgHa_Kibana.Api.Client.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Bc.CyberSec.Detection.Booster.Api.Core.QueryService;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Infrastructure;

public static class DependencyExtension
{
    public static void AddMongoDb(this IServiceCollection services, string? connectionString)
    {
        var client = new MongoClient(connectionString);
        services.AddSingleton<IMongoClient, MongoClient>(_ => client);
#pragma warning disable CS0618
        BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            return mongoClient.GetDatabase("usecase");
        });
    }

    public static IServiceCollection AddDetectionBoosterCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IUseCaseSerializerService, UseCaseSerializerService>();
        services.AddTransient<IKibanaUseCaseService, KibanaUseCaseService>();
        services.AddTransient<IUseCaseHandlerService, UseCaseHandlerService>();

        services.AddSingleton<IKibanaApi, KibanaApi>(provider => new KibanaApi(
            Environment.GetEnvironmentVariable("KIBANA_URL") ?? configuration["KIBANA_URL"],
            Environment.GetEnvironmentVariable("KIBANA_API_ACCESS_KEY") ?? configuration["KIBANA_API_ACCESS_KEY"]
        ));
        services.AddSingleton<DbContext>();
        services.AddTransient<IUseCaseQueryService, UseCaseQueryService>();
        services.AddTransient<IUseCaseToFilterBuilder, UseCaseToFilterBuilder>(provider =>
                new UseCaseToFilterBuilder(
                    Environment.GetEnvironmentVariable("CISCO_DEVICE_IPS")!.Split(',').ToList() ?? configuration["CISCO_DEVICE_IPS"]!.Split(',').ToList(),
                    Environment.GetEnvironmentVariable("FORTIGATE_IP") ?? configuration["FORTIGATE_IP"]
                )
            );
        services.AddTransient<IUseCaseDecompose, UseCaseDecompose>();

        services.AddSingleton<ISyslogNgUseCaseService>(x => new SyslogNgUseCaseService(
            x.GetService<IUseCaseSerializerService>(),
            Environment.GetEnvironmentVariable("SYSLOG_NG_CONFIG_FILE") ??
            configuration["SYSLOG_NG_CONFIG_FILE"],
            x.GetService<IUseCaseDecompose>()!
        ));

        return services;
    }
}