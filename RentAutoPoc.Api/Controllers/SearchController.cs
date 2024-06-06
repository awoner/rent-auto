using Microsoft.AspNetCore.Mvc;
using Nest;
using RentAutoPoc.Api.Entities;

namespace RentAutoPoc.Api.Controllers;

[Route("api/v1/search")]
public class SearchController : ControllerBase
{
    private const float LongWordLength = 7.0f;
    private const float AllowedTyposCount = 3.0f;
    
    private readonly IElasticClient _elasticClient;

    public SearchController(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
    }

    [HttpPost("{query}")]
    public async Task<IActionResult> Get([FromRoute] string query)
    {
        var searchResponse = await _elasticClient.SearchAsync<VocabularyRecord>(s => s
            .Index("vocabulary")
            .TrackTotalHits()
            .Query(q =>
            {
                return query.Length switch
                {
                    < (int)LongWordLength =>
                        q.Bool(b => b
                            .Should(s => s
                                .Match(m => m
                                    .Field(f => f.Word)
                                    .Query(query)
                                    .Fuzziness(Fuzziness.Auto)
                                    .PrefixLength(query.Length)
                                    .MinimumShouldMatch(1)))
                            .MinimumShouldMatch(1)),
                    >= (int)LongWordLength =>
                        q.Bool(b => b
                            .Should(s => s
                                .Match(m => m
                                    .Field(f => f.Word)
                                    .Query(query)
                                    .PrefixLength(query.Length)
                                    .MinimumShouldMatch(GetMinimumShouldMatch(query))))
                            .MinimumShouldMatch(1))
                };
            })
        );

        if (!searchResponse.IsValid)
        {
            return BadRequest(searchResponse.ServerError);
        }
        
        return Ok(searchResponse.Documents);
    }

    private static string GetMinimumShouldMatch(string query)
    {
        float wordLength = query.Length;
        return $"{(int)((wordLength - AllowedTyposCount) / wordLength * 100f)}%";
    }
}