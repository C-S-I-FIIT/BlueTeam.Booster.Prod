using Bc.CyberSec.Detection.Booster.Api.Core.Model.UseCase;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Bc.CyberSec.Detection.Booster.Api.Core.Data;

public class DbContext
{
    public IDocumentRepository<UseCase> UseCases { get; }

    public DbContext(IMongoDatabase database)
    {
        UseCases = new DocumentRepository<UseCase>(database.GetCollection<UseCase>(DbCollection.UseCase));
    }

}