using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Infrastructure.Auth;

public class ApiKey : IApiKey
{
    public ApiKey(string key)
    {
        Key = key;
    }

    public string Key { get; }
    public string OwnerName { get; }
    public IReadOnlyCollection<Claim> Claims { get; }
}