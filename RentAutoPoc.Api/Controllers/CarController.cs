using Microsoft.AspNetCore.Mvc;
using Nest;
using RentAutoPoc.Api.Entities;

namespace RentAutoPoc.Api.Controllers;

[Route("api/v1/cars")]
public class CarController : ControllerBase
{
    private static readonly Random Random = new Random();
    
    private readonly IElasticClient _elasticClient;

    public CarController(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var searchResponse = await _elasticClient.SearchAsync<Car>(s => s
                .Index("cars")
                .Query(q => q.MatchAll())
        );

        return Ok(searchResponse.Documents);
    }

    [HttpPut]
    public async Task<IActionResult> Add()
    {
        var car = new Car
        {
            Id = Guid.NewGuid(),
            Model = "Model" + Guid.NewGuid(),
            ModelYear = Random.Next(1950, 2024)
        };
        
        await _elasticClient.IndexAsync(car, i => i
            .Id(car.Id)
            .Index("cars")
        );
        
        return NoContent();
    }
}