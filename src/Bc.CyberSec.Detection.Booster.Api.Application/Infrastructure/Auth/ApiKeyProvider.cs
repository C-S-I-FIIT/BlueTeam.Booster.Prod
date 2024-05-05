using AspNetCore.Authentication.ApiKey;

namespace Bc.CyberSec.Detection.Booster.Api.Application.Infrastructure.Auth;

public class ApiKeyProvider : IApiKeyProvider
{
    private static readonly string ApiAccessKey = Environment.GetEnvironmentVariable("API_ACCESS_KEY") ?? throw new InvalidOperationException();

    public Task<IApiKey> ProvideAsync(string key)
    {
        if (key.Equals(ApiAccessKey))
        {
            return Task.FromResult<IApiKey>(new ApiKey(key));
        }

        return Task.FromResult<IApiKey>(null);
    }
}