namespace Bc.CyberSec.Detection.Booster.Api.Core.Model.UseCase;

public interface IUseCaseWithNextMatch
{
    public IUseCaseWithNextMatch WithNextMatchCondition(List<string> mnemonic);
    public string Build();
}

public interface IUseCaseWithFirstMatch
{
    public IUseCaseWithNextMatch WithFirstMatchCondition(List<string> mnemonic);
}

public interface IUseCaseToFilterBuilder
{
    public IUseCaseWithFirstMatch WithFilterDefinition();
}


public class UseCaseToFilterBuilder : IUseCaseWithFirstMatch, IUseCaseWithNextMatch, IUseCaseToFilterBuilder
{
    private string _filterConfiguration = "";

    public IUseCaseWithNextMatch WithFirstMatchCondition(List<string> mnemonic)
    {
        for (int i = 0; i < mnemonic.Count; i++)
        {
            _filterConfiguration += i == 0 ? $"match(\"{mnemonic[i]}\" value(\"MESSAGE\"))" : $" or match(\"{mnemonic[i]}\" value(\"MESSAGE\"))";
        }
        return this;
    }

    public IUseCaseWithNextMatch WithNextMatchCondition(List<string> mnemonics)
    {
        foreach (var mnemonic in mnemonics)
            _filterConfiguration += $" or match(\"{mnemonic}\" value(\"MESSAGE\"))";

        return this;
    }

    public string Build()
    {
        return _filterConfiguration + ";\n};\n";
    }

    public IUseCaseWithFirstMatch WithFilterDefinition()
    {
        _filterConfiguration = "filter f_uc_combined {\n";
        return this;
    }
}