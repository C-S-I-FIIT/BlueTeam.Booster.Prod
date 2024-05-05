namespace Bc.CyberSec.Detection.Booster.Api.Application.Model;

public class UseCase
{
    public List<string>? Mnemonics { get; set; }
    public Guid? RuleId { get; set; }
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string Name { get; set; }
}