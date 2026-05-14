using System.Text.Json;
using Bajol.GovFlow.Application.Search;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bajol.GovFlow.Infrastructure.Search;

public sealed class ElasticsearchSearchGateway(
    IOptions<ElasticsearchOptions> options,
    ILogger<ElasticsearchSearchGateway> logger) : ISearchGateway
{
    private readonly ElasticsearchOptions _options = options.Value;

    public async Task IndexJsonDocumentAsync(string indexName, string documentId, string json, CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled || _options.Uri is null)
        {
            return;
        }

        try
        {
            var settings = new ElasticsearchClientSettings(_options.Uri);
            var client = new ElasticsearchClient(settings);

            using var document = JsonDocument.Parse(json);
            var response = await client.IndexAsync(
                document.RootElement,
                idx => idx.Index(indexName).Id(documentId),
                cancellationToken);

            if (!response.IsValidResponse)
            {
                logger.LogWarning(
                    "Elasticsearch indexing failed for index {Index} id {Id}. Debug={Debug}",
                    indexName,
                    documentId,
                    response.DebugInformation);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Unexpected error indexing document {Index}/{Id}", indexName, documentId);
        }
    }
}
