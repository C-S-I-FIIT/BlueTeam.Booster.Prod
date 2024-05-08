using Bc.CyberSec.Detection.Booster.Api.Core.Model.UseCase;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Application.SyslogNg;

public interface IUseCaseDecompose
{
    string ToFilter(List<UseCase> useCases);
}

public class UseCaseDecompose : IUseCaseDecompose
{
    private IUseCaseToFilterBuilder FilterBuilder;

    public UseCaseDecompose(IUseCaseToFilterBuilder filterBuilder)
    {
        FilterBuilder = filterBuilder;
    }
    
    public string ToFilter(List<UseCase> useCases)
    {
        var tmp = FilterBuilder.Start()
            .WithFortigateFilterDefinition()
            .WithCiscoFilterDefinition()
            .WithCiscoFilterNetmaskDefinition()
            .WithCiscoFilterFirstMatchCondition(useCases!.First().Mnemonics!);

        var tmpList = useCases.Skip(1).ToList();

        foreach (var useCase in tmpList)
        {
            tmp = tmp.WithCiscoNextMatchCondition(useCase!.Mnemonics!);
        }

        return tmp.Build();
    }
}