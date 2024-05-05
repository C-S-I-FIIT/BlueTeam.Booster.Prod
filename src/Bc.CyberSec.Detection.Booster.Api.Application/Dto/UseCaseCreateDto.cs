namespace Bc.CyberSec.Detection.Booster.Api.Application.Dto;

public class UseCaseCreateDto
{
    public List<string>? Mnemonics { get; set; }
    public string? RuleId { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
}