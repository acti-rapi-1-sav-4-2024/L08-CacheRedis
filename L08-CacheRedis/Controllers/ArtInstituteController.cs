using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace L08_CacheRedis.Controllers;

[ApiController]
[Route("[controller]")]
public class ArtInstituteController : ControllerBase
{
    
    private readonly ILogger<ArtInstituteController> _logger;
    private readonly IArtInstituteService _service;
    private readonly IDatabase _redis;

    public ArtInstituteController(ILogger<ArtInstituteController> logger, IArtInstituteService service, IConnectionMultiplexer muxer)
    {
        _logger = logger;
        _service = service;
        _redis = muxer.GetDatabase();
    }

    [HttpGet(Name = "GetArtWorks")]
    public async Task<JsonObject> Get()
    {
        string json;
        
        var keyName = "ArtWorks";
        
        json = await _redis.StringGetAsync(keyName);

        if (string.IsNullOrEmpty(json))
        {
            var queryResult = _service.GetArtWorks();
            json = JsonSerializer.Serialize(queryResult);
            var setTask = _redis.StringSetAsync(keyName, json);
            var expireTask = _redis.KeyExpireAsync(keyName, TimeSpan.FromSeconds(6));
            await Task.WhenAll(setTask, expireTask);
            
        }

        var result = JsonSerializer.Deserialize<JsonObject>(json);
        return result;
    }
}