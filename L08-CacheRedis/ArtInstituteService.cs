using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace L08_CacheRedis;

public class ArtInstituteService: IArtInstituteService
{
    private readonly HttpClient _client;
    public ArtInstituteService(HttpClient client)
    {
        _client = client;
        _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("artInstituteApp", "1.0"));
    }
    
    public async Task<JsonObject> GetArtWorks()
    {
        
        var result = await _client.GetFromJsonAsync<JsonObject>("https://api.artic.edu/api/v1/artworks?page=1&limit=100");
        
        Console.WriteLine(result["data"]);
        return result;
    }
}