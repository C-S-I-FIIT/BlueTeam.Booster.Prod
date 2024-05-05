using Bc.CyberSec.Detection.Booster.Api.Core.Model.UseCase;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Application.SyslogNg;

public class UseCaseDecompose
{
    private IUseCaseToFilterBuilder FilterBuilder => new UseCaseToFilterBuilder();
    
    public string ToFilter(List<UseCase> useCases)
    {
        var tmp = FilterBuilder.WithFilterDefinition().WithFirstMatchCondition(useCases!.First().Mnemonics!);

        var tmpList = useCases.Skip(1).ToList();

        foreach (var useCase in tmpList)
        {
            tmp = tmp.WithNextMatchCondition(useCase!.Mnemonics!);
        }

        return tmp.Build();
    }
}