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

public interface IUseCaseWithNetmaskDefinition
{
    IUseCaseWithFirstMatch WithNetmaskDefinition();
}

public interface IUseCaseToFilterBuilder
{
    public IUseCaseWithNetmaskDefinition WithFilterDefinition();
}


public class UseCaseToFilterBuilder : IUseCaseWithFirstMatch, IUseCaseWithNextMatch, IUseCaseToFilterBuilder, IUseCaseWithNetmaskDefinition
{
    private List<string> CiscoDevIps;
    private string _filterConfiguration = "";

    public UseCaseToFilterBuilder(List<string> ciscoDevIps)
    {
        CiscoDevIps = ciscoDevIps;
    }

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

    public IUseCaseWithFirstMatch WithNetmaskDefinition()
    {
        for (int i = 0; i < CiscoDevIps.Count; i++)
        {
            if (i == CiscoDevIps.Count - 1)
            {
                if (i == 0)
                {
                    _filterConfiguration += $"netmask(\"{CiscoDevIps[i]}\");\n";
                }
                else
                {
                    _filterConfiguration += $"or netmask(\"{CiscoDevIps[i]}\");\n";
                }
                break;
            }

            _filterConfiguration += i == 0 ? $"netmask(\"{CiscoDevIps[i]}\")\n" : $" or netmask(\"{CiscoDevIps[i]}\")";
        }
        return this;
    }

    public IUseCaseWithNetmaskDefinition WithFilterDefinition()
    {
        _filterConfiguration = "filter f_uc_combined {\n";
        return this;
    }

    public string Build()
    {
        return _filterConfiguration + ";\n};\n";
    }
}