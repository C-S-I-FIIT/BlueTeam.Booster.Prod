using System.Text.Json.Serialization;

namespace Bc.CyberSec.Detection.Booster.Api.Client.Dto.SyslogNgConfigurator;

public class UseCaseGetDto
{
    [JsonPropertyName("mnemonics")]
    public List<string>? Mnemonics { get; set; }

    [JsonPropertyName("ruleId")]
    public Guid? KibanaRuleId { get; set; }

    [JsonPropertyName("id")]
    public string? UseCaseIdentifier { get; set; }

    [JsonPropertyName("mitreAttackId")]
    public string MitreAttackId { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
}