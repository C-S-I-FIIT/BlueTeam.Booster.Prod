using Bc.CyberSec.Detection.Booster.Api.Core.Application.Serialization;
using Bc.CyberSec.Detection.Booster.Api.Core.Model.UseCase;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Application.SyslogNg;

public interface ISyslogNgUseCaseService
{
    public Task Handle();
}

public class SyslogNgUseCaseService: ISyslogNgUseCaseService
{
    private readonly IUseCaseSerializerService _useCaseSerializerService;
    private readonly UseCaseDecompose _useCaseDecompose = new();

    private readonly string _syslogNgConfigFile;
    public SyslogNgUseCaseService(IUseCaseSerializerService? useCaseSerializerService, string? syslogNgConfigFile)
    {
        _useCaseSerializerService = useCaseSerializerService;
        _syslogNgConfigFile = syslogNgConfigFile;
        if (_syslogNgConfigFile == null)
            throw new ArgumentNullException("Provide syslog ng file document");
    }

    private void Write(List<UseCase> toFilter)
    {
        var configuration = !toFilter!.Any() ? string.Empty : _useCaseDecompose.ToFilter(toFilter.ToList());
        File.WriteAllText(_syslogNgConfigFile, configuration);
    }

    public async Task Handle()
    {
        var filteredSyslogNg = (await _useCaseSerializerService.GetUseCases())
            .Where(uc => 
                         uc is { Mnemonics: not null, IsActive: true }
                         && 
                         uc.Mnemonics.Count != 0);
        
        Write(filteredSyslogNg.ToList());
    }
}