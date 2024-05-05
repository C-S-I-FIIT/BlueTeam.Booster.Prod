using AspNetCore.Authentication.ApiKey;
using Microsoft.Extensions.Configuration;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Infrastructure.Auth;

public class ApiKeyProvider : IApiKeyProvider
{
    private readonly string ApiAccessKey;

    public ApiKeyProvider (IConfiguration configuration){
        ApiAccessKey = configuration["API_ACCESS_KEY"] ?? Environment.GetEnvironmentVariable("API_ACCESS_KEY");
        if (string.IsNullOrEmpty(ApiAccessKey))
        {
            throw new ArgumentNullException("API_ACCESS_KEY is not set");
        }
    }

    public Task<IApiKey> ProvideAsync(string key)
    {
        if (key.Equals(ApiAccessKey))
        {
            return Task.FromResult<IApiKey>(new ApiKey(key));
        }

        return Task.FromResult<IApiKey>(null);
    }
}