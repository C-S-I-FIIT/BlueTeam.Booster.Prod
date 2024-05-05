using Bc.CyberSec.Detection.Booster.Api.Application.Application.Kibana;
using Bc.CyberSec.Detection.Booster.Api.Application.Application.Serialization;
using Bc.CyberSec.Detection.Booster.Api.Application.Application.SyslogNg;
using Bc.CyberSec.Detection.Booster.Api.Application.Dto;

namespace Bc.CyberSec.Detection.Booster.Api.Application.Application;

public interface IUseCaseHandlerService
{
    public void Activate(List<UseCaseDto> useCasesToActivate);
    public void Deactivate(List<UseCaseDto> useCasesToDeactivate);
}

public class UseCaseHandlerService: IUseCaseHandlerService
{
    private readonly IUseCaseSerializerService _useCaseSerializerService;
    private readonly IKibanaUseCaseService _kibanaUseCaseService;
    private readonly ISyslogNgUseCaseService _syslogNgUseCaseService;

    public UseCaseHandlerService(IUseCaseSerializerService useCaseSerializerService, 
        IKibanaUseCaseService kibanaUseCaseService, 
        ISyslogNgUseCaseService syslogNgUseCaseService)
    {
        _useCaseSerializerService = useCaseSerializerService;
        _kibanaUseCaseService = kibanaUseCaseService;
        _syslogNgUseCaseService = syslogNgUseCaseService;
    }

    public void Activate(List<UseCaseDto> useCasesToActivate)
    {
        _useCaseSerializerService.UpdateCache(useCasesToActivate, true);

        _syslogNgUseCaseService.Handle();
        _kibanaUseCaseService.Activate(useCasesToActivate);
    }

    public void Deactivate(List<UseCaseDto> useCasesToDeactivate)
    {
        _useCaseSerializerService.UpdateCache(useCasesToDeactivate, false);

        _syslogNgUseCaseService.Handle();
        _kibanaUseCaseService.Deactivate(useCasesToDeactivate);
    }
}