using Bc.CyberSec.Detection.Booster.Api.Application.Application.Serialization;
using Bc.CyberSec.Detection.Booster.Api.Application.Model;
using Bc.CyberSec.Detection.Booster.Api.Client.Dto.Kibana;
using Bc.SyslogNgHa_Kibana.Api.Client.Api;
using ActivateDeactivateDto = Bc.CyberSec.Detection.Booster.Api.Application.Dto.UseCaseDto;

namespace Bc.CyberSec.Detection.Booster.Api.Application.Application.Kibana;

public interface IKibanaUseCaseService
{
    public Task Activate(List<ActivateDeactivateDto> useCases);
    public Task Deactivate(List<ActivateDeactivateDto> useCases);
}

public class KibanaUseCaseService: IKibanaUseCaseService
{
    private readonly IKibanaApi _api;
    private readonly IUseCaseSerializerService _useCaseSerializerService;

    public KibanaUseCaseService(
        IKibanaApi api,
        IUseCaseSerializerService useCaseSerializerService
        )
        
    {
        _api = api;
        _useCaseSerializerService = useCaseSerializerService;
    }

    private List<UseCaseDto> MapToDto(List<UseCase> useCases)
    {
        return useCases.Select(useCase => new UseCaseDto
        {
            RuleId = useCase!.RuleId!.Value,
        }).ToList();
    }

    public async Task Activate(List<ActivateDeactivateDto> useCases)
    {
        var filteredKibana = _useCaseSerializerService.GetUseCases()
            .Where(uc => uc.IsActive).ToList();
        _api.ActivateUseCase(MapToDto(filteredKibana));
    }

    public async Task Deactivate(List<ActivateDeactivateDto> useCases)
    {
        var filteredKibana = _useCaseSerializerService.GetUseCases()
            .Where(uc => !uc.IsActive).ToList(); 
        _api.DeactivateUseCase(MapToDto(filteredKibana));
    }
}