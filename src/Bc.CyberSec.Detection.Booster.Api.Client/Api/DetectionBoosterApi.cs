using System.Net;
using System.Text.Json;
using Bc.CyberSec.Detection.Booster.Api.Client.Dto.SyslogNgConfigurator;
using RestSharp;

namespace Bc.CyberSec.Detection.Booster.Api.Client.Api;

public interface IDetectionBoosterApi
{
    void ActivateUseCase(List<int> identifiers);
    void DeactivateUseCase(List<int> identifiers);
    HttpStatusCode CreateUseCases(List<UseCaseCreateDto> useCases);
    List<UseCaseGetDto> GetActiveUseCases();
    List<UseCaseGetDto> GetInactiveUseCases();
    DateTime? GetWhenSerialized();
    List<UseCaseGetDto> GetAllUseCases();
    string GetApiUrl();
}
public class DetectionBoosterApi: BasicApi, IDetectionBoosterApi
{
    private RestClient _client;
    private string _apiUrl;
    public DetectionBoosterApi(string apiUrl, string token) : base(apiUrl, token)
    {
        _apiUrl = apiUrl;
        _client = new RestClient(_apiUrl,
            options =>
            {
                options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                options.FollowRedirects = true;
            },
            headers =>
            {
                headers.Add("Key", _token);
            }
        );
        _apiUrl = apiUrl;
    }

    public void ActivateUseCase(List<int> identifiers)
    {

        foreach (var identifier in identifiers)
        {
            var request = new RestRequest($"/api/uc/activate/{identifier}", Method.Put);
            var response = _client.Execute(request);
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException(
                    $"Používateľský prípad s identifikátorom UC{identifier} sa nepodarilo aktivovať");
        }
    }

    public void DeactivateUseCase(List<int> identifiers)
    {
        foreach (var identifier in identifiers)
        {
            var request = new RestRequest($"/api/uc/deactivate/{identifier}", Method.Put);
            request.AddParameter("id", identifier);
            var response = _client.Execute(request);
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException(
                    $"Používateľský prípad s identifikátorom {identifier} sa nepodarilo aktivovať");
        }
    }

    public HttpStatusCode CreateUseCases(List<UseCaseCreateDto> useCases)
    {
        var request = new RestRequest("/api/uc/save", Method.Post);
        request.AddJsonBody(useCases);

        var response = _client.Execute(request);
        return response.StatusCode;
    }

    public List<UseCaseGetDto> GetActiveUseCases()
    {
        var request = new RestRequest("/api/uc/active");
        var response = _client.Execute(request);

        if (response.IsSuccessStatusCode)
            return JsonSerializer.Deserialize<List<UseCaseGetDto>>(response.Content);
        
        throw new ApplicationException(response.ErrorMessage);
    }

    public List<UseCaseGetDto> GetInactiveUseCases()
    {
        var request = new RestRequest("/api/uc/inactive");
        var response = _client.Execute(request);

        if (response.IsSuccessStatusCode)
            return JsonSerializer.Deserialize<List<UseCaseGetDto>>(response.Content);
       
        throw new ApplicationException(response.ErrorMessage);
    }

    public DateTime? GetWhenSerialized()
    {
        var request = new RestRequest("/api/uc/save");
        var response = _client.Execute(request);
        if (response.IsSuccessStatusCode)
            return JsonSerializer.Deserialize<DateTime>(response.Content);

        throw new ApplicationException(response.ErrorMessage);
    }

    public List<UseCaseGetDto> GetAllUseCases()
    {
        var request = new RestRequest("/api/uc/all");
        var response = _client.Execute(request);
        if (response.IsSuccessStatusCode)
            return JsonSerializer.Deserialize<List<UseCaseGetDto>>(response.Content);

        throw new ApplicationException(response.ErrorMessage);
    }

    public string GetApiUrl()
    {
        return _apiUrl;
    }
}