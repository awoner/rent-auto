using Elasticsearch.Net;
using Nest;
using RentAutoPoc.Api.Entities;
using RentAutoPoc.Api.Extensions;
using RentAutoPoc.Api.Infrastructure;

namespace RentAutoPoc.Api.Application.Services;

public class ElasticSeedBackgroundService : BackgroundService
{
    private readonly string _dictionariesPath = $"{PathProvider.GetBase()}/Resources/Dictionaries";

    private readonly IElasticClient _elasticClient;

    public ElasticSeedBackgroundService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DeleteAllIndicesAsync(stoppingToken);
        await CreateVocabularyIndexAsync(stoppingToken);
    }

    private async Task CreateVocabularyIndexAsync(CancellationToken stoppingToken)
    {
        var createEngWordsIndexResponse = await _elasticClient.Indices.CreateAsync(
            "vocabulary",
            c => c.Settings(
                    s => s.Analysis(
                        a => a.TokenFilters(
                                t => t.EdgeNGram(
                                    "edge_ngram_filter",
                                    e => e
                                        .MinGram(2)
                                        .MaxGram(20)))
                            .Analyzers(an => an
                                .Custom("edge_ngram_analyzer", ca => ca
                                    .Tokenizer("standard")
                                    .Filters("lowercase", "stop", "edge_ngram_filter")))))
                .Map<VocabularyRecord>(m => m
                    .AutoMap<VocabularyRecord>()
                    .Properties(p => p
                        .Text(t => t
                            .Name(n => n.Word)
                            .Analyzer("edge_ngram_analyzer")))),
            stoppingToken);

        if (!createEngWordsIndexResponse.IsValid)
        {
            return;
        }

        var words = (await File.ReadAllLinesAsync($"{_dictionariesPath}/eng.txt", stoppingToken))
            .Select(w => new VocabularyRecord
            {
                Word = w,
                Language = "English"
            });

        foreach (var batch in words.Batch(500))
            _ = await IndexWithRetriesAsync(b => b.Index("vocabulary").IndexMany(batch), 4);
    }

    private async Task<BulkResponse> IndexWithRetriesAsync(
        Func<BulkDescriptor, IBulkRequest> selector,
        int retries = 3)
    {
        for (int attempt = 0; attempt < retries; attempt++)
        {
            var response = await _elasticClient.BulkAsync(selector);

            if (response.IsValid)
            {
                return response;
            }

            if (response.OriginalException is ElasticsearchClientException clientEx &&
                clientEx.Response.HttpStatusCode == 429)
            {
                // Exponential backoff
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
            }
            else
            {
                return response;
            }
        }

        // Final attempt
        return await _elasticClient.BulkAsync(selector);
    }

    private async Task DeleteAllIndicesAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            var result = await _elasticClient.Cluster.HealthAsync(ct: stoppingToken);
            if ((result?.IsValid ?? false) && result.Status == Health.Green)
            {
                break;
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
        
        await _elasticClient.DeleteByQueryAsync<string>(
            s => s.Index("vocabulary").Query(q => q.MatchAll()), stoppingToken);
    }
}