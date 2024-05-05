using Bc.CyberSec.Detection.Booster.Api.Application.Application.Serialization;
using Bc.CyberSec.Detection.Booster.Api.Application.Model;

namespace Bc.CyberSec.Detection.Booster.Api.Application.Application.SyslogNg;

public interface ISyslogNgUseCaseService
{
    public void Handle();
}

public class SyslogNgUseCaseService: ISyslogNgUseCaseService
{
    private readonly IUseCaseSerializerService _useCaseSerializerService;
    private UseCaseDecompose _useCaseDecompose = new();

    private static readonly string SyslogNgConfigPath = Path.Combine(Environment.GetEnvironmentVariable("SYSLOG_NG_CONFIG_DIR") ?? throw new ApplicationException(),
                                                                        Environment.GetEnvironmentVariable("SYSLOG_NG_CONFIG_FILE") ?? throw new ApplicationException());
    public SyslogNgUseCaseService(IUseCaseSerializerService useCaseSerializerService)
    {
        _useCaseSerializerService = useCaseSerializerService;
    }

    private void Write(List<UseCase> toFilter)
    {
        var configuration = !toFilter!.Any() ? string.Empty : _useCaseDecompose.ToFilter(toFilter.ToList());
        File.WriteAllText(SyslogNgConfigPath, configuration);
    }

    public void Handle()
    {
        var filteredSyslogNg = _useCaseSerializerService.GetUseCases()
            .Where(uc => 
                         uc is { Mnemonics: not null, IsActive: true }
                         && 
                         uc.Mnemonics.Count != 0);
        
        Write(filteredSyslogNg.ToList());
    }
}