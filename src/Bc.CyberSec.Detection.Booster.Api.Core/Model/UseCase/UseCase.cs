using Bc.CyberSec.Detection.Booster.Api.Application.Model.Base;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Model.UseCase;

public class UseCase : CollectionRoot
{
    public List<string>? Mnemonics { get; set; }
    public Guid? RuleId { get; set; }
    public bool IsActive { get; set; }
    public string MitreAttackId { get; set; }
    public string Name { get; set; }
}