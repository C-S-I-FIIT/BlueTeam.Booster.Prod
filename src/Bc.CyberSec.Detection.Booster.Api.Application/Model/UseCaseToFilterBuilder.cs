namespace Bc.CyberSec.Detection.Booster.Api.Application.Model;

public interface IUseCaseWithFirstMatch
{
    public IUseCaseWithNextMatch WithFirstMatchCondition(UseCase useCase);
}

public interface IUseCaseWithNextMatch
{
    public IUseCaseWithNextMatch WithNextMatchCondition(UseCase useCase);
}

public interface IFilterBuilder
{
    public IUseCaseWithFirstMatch WithFilterDefinition();
}

public class UseCaseToFilterBuilder: IUseCaseWithFirstMatch, IFilterBuilder, IUseCaseWithNextMatch
{
    private string _filterConfiguration = "";
    private bool _filterDefinitionCalled;
    private bool _firstMatchCalled;


    public IUseCaseWithNextMatch WithFirstMatchCondition(UseCase useCase)
    {
        _firstMatchCalled = true;
        var matchExpressions = useCase.Mnemonics.Select(mnemonic => $"match(\"{mnemonic}\" value(\"MESSAGE\"))");
        _filterConfiguration += string.Join(" or ", matchExpressions);

        return this;
    }

    public IUseCaseWithNextMatch WithNextMatchCondition(UseCase useCase)
    {
        foreach (var mnemonic in useCase.Mnemonics)
            _filterConfiguration += $" or match(\"{mnemonic}\" value(\"MESSAGE\"))";

        return this;
    }

    public IUseCaseWithFirstMatch WithFilterDefinition()
    {
        _filterDefinitionCalled = true;
        
        _filterConfiguration = "filter f_uc_combined {\n";
        return this;
    }

    public string Build()
    {
        if(!_filterDefinitionCalled || !_firstMatchCalled)
            throw new InvalidOperationException("Filter definition not called");

        return _filterConfiguration + ";\n};\n";
    }

    
}