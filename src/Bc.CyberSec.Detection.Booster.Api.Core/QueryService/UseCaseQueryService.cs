using Bc.CyberSec.Detection.Booster.Api.Client.Dto.SyslogNgConfigurator;
using Bc.CyberSec.Detection.Booster.Api.Core.Data;
using Bc.CyberSec.Detection.Booster.Api.Core.Model.UseCase;
using MongoDB.Driver;

namespace Bc.CyberSec.Detection.Booster.Api.Core.QueryService;

public interface IUseCaseQueryService
{
    Task<List<UseCaseGetDto>> GetUseCases();
    Task<List<UseCase>> GetUseCasesDomain();
}

public class UseCaseQueryService : IUseCaseQueryService
{
    private IMongoCollection<UseCase> _context;
    
    public UseCaseQueryService(IMongoDatabase database)
    {
        _context = database.GetCollection<UseCase>(DbCollection.UseCase);
    }

    public async Task<List<UseCaseGetDto>> GetUseCases()
    {
        var useCases = await _context.Find(x => true).ToListAsync();

        return  MapToDto(useCases);
    }

    public async Task<List<UseCase>> GetUseCasesDomain()
    {
        var useCases = await _context.Find(x => true).ToListAsync();
        return useCases;
    }

    private List<UseCaseGetDto> MapToDto(List<UseCase> useCases)
    {
        return useCases.Select(x => new UseCaseGetDto()
        {
            UseCaseIdentifier = x.UseCaseIdentitifier,
            Name = x.Name,
            Mnemonics = x.Mnemonics,
            KibanaRuleId = x.KibanaRuleId,
            MitreAttackId = x.MitreAttackId
        }).ToList();
    }
}