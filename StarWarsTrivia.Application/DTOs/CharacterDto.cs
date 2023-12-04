using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace StarWars.Application.DTOs;

/// <summary>
/// Character DTO : Data transfer object represtation of Character
/// </summary>
public class CharacterDto
{
    /// <summary>
    /// Id.
    /// </summary>
    [BsonId]
    [BsonIgnoreIfDefault]
    public ObjectId Id { get; set; } 

    /// <summary>
    /// Name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// List of films.
    /// </summary>
    public List<string>? Films { get; set; }

    /// <summary>
    /// List of vehicles.
    /// </summary>
    public List<string>? Vehicles { get; set; }
}

