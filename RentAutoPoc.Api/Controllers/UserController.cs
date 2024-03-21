using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RentAutoPoc.Api.Entities;

namespace RentAutoPoc.Api.Controllers;

[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly IMongoDatabase _mongo;

    public UserController(IMongoDatabase mongo)
    {
        _mongo = mongo ?? throw new ArgumentNullException(nameof(mongo));
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var collection = await GetOrCreateCollection("users");
        var users = await collection.FindAsync(FilterDefinition<User>.Empty);
        return Ok(await users.ToListAsync());
    }

    private async Task<IMongoCollection<User>> GetOrCreateCollection(string name)
    {
        var collectionExists = _mongo.ListCollectionNames().ToList().Contains(name);
        if (!collectionExists)
        {
            await _mongo.CreateCollectionAsync(name);
        }
        
        return _mongo.GetCollection<User>(name);
    }

    [HttpPut]
    public async Task<IActionResult> Add()
    {
        
        var collection = await GetOrCreateCollection("users");
        var car = new User
        {
            Name = "Name" + Guid.NewGuid(),
            BirthYear = 1999,
        };
        
        await collection.InsertOneAsync(car, new InsertOneOptions(), CancellationToken.None);
        return NoContent();
    }
}