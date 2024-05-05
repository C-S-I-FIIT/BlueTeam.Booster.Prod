using Bc.CyberSec.Detection.Booster.Api.Core.Application.Kibana;
using Bc.CyberSec.Detection.Booster.Api.Core.Application.Serialization;
using Bc.CyberSec.Detection.Booster.Api.Core.Application.SyslogNg;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Application;

public interface IUseCaseHandlerService
{
    public Task Activate(string id);
    public Task Deactivate(string identifier);
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

    public async Task Activate(string identifier)
    {
        await _useCaseSerializerService.UpdateUseCaseState(identifier, true);
        var id = (await _useCaseSerializerService.GetUseCases())
            .Where(uc => uc.Identifier == identifier)
            .Select(uc => uc.RuleId).First();

        await _syslogNgUseCaseService.Handle();
        await _kibanaUseCaseService.Activate(id.Value);
    }

    public async Task Deactivate(string identifier)
    {
        await _useCaseSerializerService.UpdateUseCaseState(identifier, false);
        var id = (await _useCaseSerializerService.GetUseCases())
            .Where(uc => uc.Identifier == identifier)
            .Select(uc => uc.RuleId).First();

        await _syslogNgUseCaseService.Handle();
        await _kibanaUseCaseService.Deactivate(id.Value);
    }
}