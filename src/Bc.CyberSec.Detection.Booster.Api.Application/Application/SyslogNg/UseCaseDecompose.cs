using Bc.CyberSec.Detection.Booster.Api.Application.Model;

namespace Bc.CyberSec.Detection.Booster.Api.Application.Application.SyslogNg;

internal class UseCaseDecompose
{
    private UseCaseToFilterBuilder _filterBuilder = new();
    
    public string ToFilter(List<UseCase> useCases)
    {
        var tmp = _filterBuilder.WithFilterDefinition().WithFirstMatchCondition(useCases.First());

        var tmpList = useCases.Skip(1).ToList();

        foreach (var useCase in tmpList)
        {
            tmp = tmp.WithNextMatchCondition(useCase);
        }

        return _filterBuilder.Build();
    }
}