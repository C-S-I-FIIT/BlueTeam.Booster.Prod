namespace Bc.CyberSec.Detection.Booster.Api.Core.Model.UseCase;

public interface IUseCaseWithNextMatch
{
    public IUseCaseWithNextMatch WithCiscoNextMatchCondition(List<string> mnemonic);
    public string Build();
}

public interface IUseCaseWithFirstMatch
{
    public IUseCaseWithNextMatch WithCiscoFilterFirstMatchCondition(List<string> mnemonic);
}

public interface IUseCaseWithNetmaskDefinition
{
    IUseCaseWithFirstMatch WithCiscoFilterNetmaskDefinition();
}

public interface IUseCaseWithCiscoFilterDefinition
{
    IUseCaseWithNetmaskDefinition WithCiscoFilterDefinition();
}

public interface IUseCaseWithFortigateFilterDefinition
{
    IUseCaseWithCiscoFilterDefinition WithFortigateFilterDefinition();
}

public interface IUseCaseToFilterBuilder
{
    public IUseCaseWithFortigateFilterDefinition Start();
}


public class UseCaseToFilterBuilder : IUseCaseWithFirstMatch, IUseCaseWithNextMatch, IUseCaseToFilterBuilder, IUseCaseWithNetmaskDefinition, IUseCaseWithCiscoFilterDefinition, IUseCaseWithFortigateFilterDefinition
{
    private List<string> CiscoDevIps;
    private string FortigateIp;
    private string _filterConfiguration = "";

    public UseCaseToFilterBuilder(List<string> ciscoDevIps, string fortigateIp)
    {
        CiscoDevIps = ciscoDevIps;
        FortigateIp = fortigateIp;
    }

    public IUseCaseWithNextMatch WithCiscoFilterFirstMatchCondition(List<string> mnemonic)
    {
        for (int i = 0; i < mnemonic.Count; i++)
        {
            _filterConfiguration += i == 0 ? $"match(\"{mnemonic[i]}\" value(\"MESSAGE\"))" : $" or match(\"{mnemonic[i]}\" value(\"MESSAGE\"))";
        }
        return this;
    }

    public IUseCaseWithNextMatch WithCiscoNextMatchCondition(List<string> mnemonics)
    {
        foreach (var mnemonic in mnemonics)
            _filterConfiguration += $" or match(\"{mnemonic}\" value(\"MESSAGE\"))";

        return this;
    }

    public string Build()
    {
        return _filterConfiguration + ";\n};\n";
    }

    public IUseCaseWithFortigateFilterDefinition Start()
    {
        return this;
    }

    public IUseCaseWithFirstMatch WithCiscoFilterNetmaskDefinition()
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

    public IUseCaseWithNetmaskDefinition WithCiscoFilterDefinition()
    {
        _filterConfiguration += "filter f_uc_cisco {\n";
        return this;
    }

    public IUseCaseWithCiscoFilterDefinition WithFortigateFilterDefinition()
    {
        _filterConfiguration = "filter f_uc_fortigate {\n";
        _filterConfiguration += $"netmask(\"{FortigateIp}\");";
        _filterConfiguration += "\n};\n\n";
        return this;
    }
}