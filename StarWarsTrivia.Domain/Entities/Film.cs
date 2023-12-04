using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StarWars.Domain.Entities;

/// <summary>
/// Film : Film Domain Representation
/// </summary>
public class Film
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    public string? Title { get; set; }
    public int EpisodeId { get; set; }
    public string? OpeningCrawl { get; set; }
    public string? Director { get; set; }
    public string? Producer { get; set; }
    public DateTime ReleaseDate { get; set; }

}
