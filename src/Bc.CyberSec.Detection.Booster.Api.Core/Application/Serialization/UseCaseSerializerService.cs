using System.Text.Json;
using Bc.CyberSec.Detection.Booster.Api.Core.Data;
using Bc.CyberSec.Detection.Booster.Api.Core.Dto;
using Bc.CyberSec.Detection.Booster.Api.Core.Model.UseCase;
using Bc.CyberSec.Detection.Booster.Api.Core.QueryService;
using Microsoft.Extensions.Caching.Memory;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Application.Serialization;

public interface IUseCaseSerializerService
{
    public Task<List<UseCase>> GetUseCases();
    public Task Save(List<UseCaseCreateDto> useCasesDto);
    public Task Save(List<UseCase> useCases);
    public Task UpdateUseCaseState(string id, bool state);
    public DateTime WhenSerialized();
}

public class UseCaseSerializerService : IUseCaseSerializerService
{
    private DbContext _context;
    private readonly string _cacheObjectsKey = "cachedUseCases";
    private readonly string _cacheWhenKey = "cached";
    private readonly IMemoryCache _cache;
    private readonly IUseCaseQueryService _queryService;

    public UseCaseSerializerService(IMemoryCache cache, DbContext context, IUseCaseQueryService queryService)
    {
        _cache = cache;
        _context = context;
        _queryService = queryService;
    }

    public async Task<List<UseCase>> GetUseCases()
    {
        if (_cache.Get<List<UseCase>>(_cacheObjectsKey) == null)
        {
            await _queryService.GetUseCasesDomain();
        }

        return _cache.Get<List<UseCase>>(_cacheObjectsKey) ?? await _queryService.GetUseCasesDomain();
    }

    public async Task Save(List<UseCaseCreateDto> useCasesDto)
    {
        var createdAt = DateTime.Now;
        var useCases = MapToUseCase(useCasesDto);
        _cache.Set(_cacheObjectsKey, useCases);
        _cache.Set(_cacheWhenKey, createdAt);
        await Save(useCases);
    }

    public async Task Save(List<UseCase> useCases)
    {
        await _context.UseCases.RemoveAllUseCase();

        foreach (var useCase in useCases)
        {
            await _context.UseCases.UpdateAsync(useCase);
        }
    }

    public async Task UpdateUseCaseState(string id, bool state)
    {
        var cachedUc = await GetUseCases();

        foreach (var useCase in cachedUc)
        {
            if (useCase.UseCaseIdentitifier.Equals(id))
            {
                useCase.IsActive = state;
                await _context.UseCases.UpdateAsync(useCase);
            }
        }

        _cache.Set(_cacheObjectsKey, cachedUc);
    }

    public DateTime WhenSerialized()
    {
        var when = _cache.Get<DateTime>(_cacheWhenKey);
        return when;
    }

    private List<UseCase> MapToUseCase(List<UseCaseCreateDto> useCasesDto)
    {
        return useCasesDto.Select(ucDto => new UseCase
        {
            Mnemonics = ucDto.Mnemonics,
            Id = Guid.NewGuid(),
            UseCaseIdentitifier = ucDto.UseCaseIdentifier,
            KibanaRuleId = string.IsNullOrEmpty(ucDto.KibanaRuleId) ? Guid.Empty : new Guid(ucDto.KibanaRuleId),
            IsActive = false,
            Name = ucDto.Name,
            MitreAttackId = ucDto.MitreAttackId
        }).ToList();
    }
}