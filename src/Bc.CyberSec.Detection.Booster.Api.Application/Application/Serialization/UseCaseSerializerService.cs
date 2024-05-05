using System.Text.Json;
using Bc.CyberSec.Detection.Booster.Api.Application.Dto;
using Bc.CyberSec.Detection.Booster.Api.Application.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Bc.CyberSec.Detection.Booster.Api.Application.Application.Serialization;

public interface IUseCaseSerializerService
{
    public List<UseCase> GetUseCases();
    public void Save(List<UseCaseCreateDto> useCasesDto);
    public void Save(List<UseCase> useCases);
    public void UpdateCache(List<UseCaseDto> useCases, bool state);
    public DateTime WhenSerialized();
}

public class UseCaseSerializerService : IUseCaseSerializerService
{
    private readonly string _filePath = Path.Combine(Path.GetTempPath(), Environment.GetEnvironmentVariable("SERIALIZE_USECASES_FILE") ?? throw new ApplicationException("Provide fileName for serialization"));
    private readonly string _cacheObjectsKey = "cachedUseCases";
    private readonly string _cacheWhenKey = "cached";
    private readonly IMemoryCache _cache;

    public UseCaseSerializerService(IMemoryCache cache)
    {
        _filePath = Path.Combine(Path.GetTempPath(), _filePath);
        _cache = cache;
    }

    public List<UseCase> GetUseCases()
    {
        return _cache.Get<List<UseCase>>(_cacheObjectsKey) ?? DeserializeUseCases().UseCases;
    }

    public void Save(List<UseCaseCreateDto> useCasesDto)
    {
        var createdAt = DateTime.Now;
        var useCases = MapToUseCase(useCasesDto);
        _cache.Set(_cacheObjectsKey, useCases);
        _cache.Set(_cacheWhenKey, createdAt);
        SerializeToFile(useCases, createdAt);
    }

    public void Save(List<UseCase> useCases)
    {
        var savedAt = DateTime.Now;
        SerializeToFile(useCases, savedAt);
    }

    public void UpdateCache(List<UseCaseDto> useCases, bool state)
    {
        var cachedUc = GetUseCases();

        cachedUc.ForEach(uc =>
        {
            if (useCases.Any(ucChange => uc.Id.Equals(ucChange.Id)))
                uc.IsActive = state;
        });
        _cache.Set(_cacheObjectsKey, cachedUc);
    }

    public DateTime WhenSerialized()
    {
        var when = _cache.Get<DateTime>(_cacheWhenKey);
        if (when == null)
        {
            return DeserializeUseCases().CreatedAt;
        }

        return when;
    }

    private List<UseCase> MapToUseCase(List<UseCaseCreateDto> useCasesDto)
    {
        return useCasesDto.Select(ucDto => new UseCase
        {
            Mnemonics = ucDto.Mnemonics,
            Id = ucDto.Id,
            RuleId = string.IsNullOrEmpty(ucDto.RuleId) ? Guid.Empty : new Guid(ucDto.RuleId),
            IsActive = false,
            Name = ucDto.Name
        }).ToList();
    }

    private void SerializeToFile(List<UseCase> useCases, DateTime WhenSerialized)
    {
        string jsonString = JsonSerializer.Serialize(new UseCaseSerialized()
        {
            UseCases = useCases,
            CreatedAt = WhenSerialized
        }, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonString);
    }

    private UseCaseSerialized DeserializeUseCases()
    {
        string jsonString = File.ReadAllText(_filePath);
        UseCaseSerialized useCases = JsonSerializer.Deserialize<UseCaseSerialized>(jsonString);
        if (useCases == null || useCases.UseCases.Count is 0)
            throw new ApplicationException("Use cases are not serialized");

        return useCases;
    }

}