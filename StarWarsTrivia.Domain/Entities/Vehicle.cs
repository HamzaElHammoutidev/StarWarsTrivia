using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StarWars.Domain.Entities;

/// <summary>
/// Vehicle Domain Representation
/// </summary>
public class Vehicle
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Model { get; set; }
    public string? Manufacturer { get; set; }
    public string? CostInCredits { get; set; }
    public string? Length { get; set; }
    public string? MaxAtmospheringSpeed { get; set; }
    public int Crew { get; set; }
    public int Passengers { get; set; }
    public string? CargoCapacity { get; set; }
    public string? Consumables { get; set; }
    public string? VehicleClass { get; set; }

}
