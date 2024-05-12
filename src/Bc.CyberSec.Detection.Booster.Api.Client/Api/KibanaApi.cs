using Bc.SyslogNgHa_Kibana.Api.Client.Client;
using System.Net.Http.Headers;
using RestSharp;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using Bc.CyberSec.Detection.Booster.Api.Client.Dto.Kibana;

namespace Bc.SyslogNgHa_Kibana.Api.Client.Api;

public interface IKibanaApi
{
    void CreateUseCase(List<UseCaseDto> useCases);
    void ActivateUseCase(Guid id);
    void DeactivateUseCase(Guid id);
    void DeactivateRule(Guid id);
}
public class KibanaApi: BasicApi, IKibanaApi
{
    private RestClient _client;
    public KibanaApi(string? apiUrl, string? token) : base(apiUrl, token)
    {
        _client = new RestClient(_apiUrl,
            options =>
            {
                options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                options.FollowRedirects = true;
            },
            headers =>
            {
                headers.Add("Authorization", $"ApiKey {_token}");
                headers.Add("kbn-xsrf", "true");
            }
        );
    }

    private void SendRequest(string uri, Guid id)
    {
            var request = new RestRequest($"{id}/{uri}", Method.Post);
            var response = _client.Execute(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException("Kibana Api call failed");
            }
    }

    public void CreateUseCase(List<UseCaseDto> useCases)
    {

    }

    public void ActivateUseCase(Guid id)
    {
        SendRequest("_enable", id);
    }

    public void DeactivateUseCase(Guid id)
    {
       SendRequest("_disable", id);
    }

    public void DeactivateRule(Guid id)
    {
        SendRequest("_disable", id);
    }
}