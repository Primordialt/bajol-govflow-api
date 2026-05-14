namespace Bajol.GovFlow.Infrastructure.Search;

public sealed class ElasticsearchOptions
{
    public const string SectionName = "Elasticsearch";

    public bool Enabled { get; init; }
    public Uri? Uri { get; init; }
}
