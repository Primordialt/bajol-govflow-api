namespace Bajol.GovFlow.Application.Search;

public interface ISearchGateway
{
    Task IndexJsonDocumentAsync(string indexName, string documentId, string json, CancellationToken cancellationToken = default);
}
