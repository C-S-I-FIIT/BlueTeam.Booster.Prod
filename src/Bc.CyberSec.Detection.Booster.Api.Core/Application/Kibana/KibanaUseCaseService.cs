using Bc.CyberSec.Detection.Booster.Api.Client.Dto.Kibana;
using Bc.CyberSec.Detection.Booster.Api.Core.Application.Serialization;
using Bc.CyberSec.Detection.Booster.Api.Core.Model.UseCase;
using Bc.SyslogNgHa_Kibana.Api.Client.Api;
using ActivateDeactivateDto = Bc.CyberSec.Detection.Booster.Api.Core.Dto.UseCaseDto;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Application.Kibana;

public interface IKibanaUseCaseService
{
    public Task Activate(Guid id);
    public Task Deactivate(Guid id);
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

    public async Task Activate(Guid id)
    {
        _api.ActivateUseCase(id);
    }

    public async Task Deactivate(Guid id)
    {
        _api.DeactivateUseCase(id);
    }
}