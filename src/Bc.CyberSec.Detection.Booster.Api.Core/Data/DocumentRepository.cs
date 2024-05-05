using System.Security.Cryptography;
using Bc.CyberSec.Detection.Booster.Api.Application.Model.Base;
using MongoDB.Driver;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Data;

public interface IDocumentRepository<T> where T : CollectionRoot
{
    Task<T> LoadByIdAsync(string id);
    Task<T?> LoadByIdIfExistAsync(string id);
    Task UpdateAsync(T document);
    Task DeleteAsync(string id);
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
        return await Collection.Find(x => x.Identifier.Equals(id)).FirstAsync();
    }

    public async Task<T?> LoadByIdIfExistAsync(string id)
    {
        return await Collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(T document)
    {
        await Collection.ReplaceOneAsync(x => x.Identifier.Equals(document.Identifier),
            document, new ReplaceOptions() { IsUpsert = true });
    }

    public async Task DeleteAsync(string id)
    {
        await Collection.DeleteOneAsync( x => x.Identifier.Equals(id));
    }
}