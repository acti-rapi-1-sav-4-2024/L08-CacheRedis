using System.Text.Json.Nodes;

namespace L08_CacheRedis;

public interface IArtInstituteService
{
    public Task<JsonObject> GetArtWorks();
}