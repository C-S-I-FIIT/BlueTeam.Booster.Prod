using RestSharp;

namespace Bc.CyberSec.Detection.Booster.Api.Client;

public class BasicApi
{
    protected readonly string _apiUrl;
    protected readonly string _token;
    
    public BasicApi(string apiUrl, string token)
    {
        _apiUrl = apiUrl;
        _token = token;
    }

    
}