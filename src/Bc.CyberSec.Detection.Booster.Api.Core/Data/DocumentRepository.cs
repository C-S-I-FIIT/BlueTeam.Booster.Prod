using Bc.CyberSec.Detection.Booster.Api.Application.Model.Base;
using MongoDB.Driver;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Data;

public interface IDocumentRepository<T> where T : CollectionRoot
{
    Task<T> LoadByIdAsync(string id);
    Task<T?> LoadByIdIfExistAsync(string id);
    Task UpdateAsync(T document);
    Task DeleteAsync(string id);
    Task RemoveAllUseCase();
}

public class DocumentRepository<T> : IDocumentRepository<T> where T : CollectionRoot
{
    protected readonly IMongoCollection<T> Collection;

    public DocumentRepository(IMongoCollection<T> collection)
    {
        Collection = collection;
    }

    public async Task<T> LoadByIdAsync(string id)
    {
        return await Collection.Find(x => x.UseCaseIdentitifier.Equals(id)).FirstAsync();
    }

    public async Task<T?> LoadByIdIfExistAsync(string id)
    {
        return await Collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(T document)
    {
        await Collection.ReplaceOneAsync(x => x.UseCaseIdentitifier.Equals(document.UseCaseIdentitifier),
            document, new ReplaceOptions() { IsUpsert = true });
    }

    public async Task DeleteAsync(string id)
    {
        await Collection.DeleteOneAsync( x => x.UseCaseIdentitifier.Equals(id));
    }

    public async Task RemoveAllUseCase()
    {
        await Collection.DeleteManyAsync(x => true);
    }
}