
using MongoDB.Bson;
using MongoDB.Driver;
using StarWars.Application.DTOs;
using StarWars.Application.Interfaces;

namespace StarWars.Infrastructure.Services;


/// <summary>
/// MongoDbService : Implementation of IMongoDbService for a service that interacts with MongoDB.
/// </summary>
public class MongoDbService : IMongoDbService
{
    /// <summary>
    /// Characters Collection
    /// </summary>
    private readonly IMongoCollection<CharacterDto> _charactersCollection;

    /// <summary>
    /// MongoDbService : Constructor
    /// </summary>
    /// <param name="database"></param>
    public MongoDbService(IMongoDatabase database)
    {
        _charactersCollection = database.GetCollection<CharacterDto>("characters");
    }

    /// <summary>
    /// GetDataAsync : retrieves character data based on a name.
    /// </summary>
    /// <param name="key">name of character</param>
    /// <returns></returns>
    public async Task<List<CharacterDto>> GetDataAsync(string key)
    {
        var filter = Builders<CharacterDto>.Filter.Regex("Name", new BsonRegularExpression(key, "i"));

        return await _charactersCollection.Find(filter).ToListAsync();

    }

    /// <summary>
    /// SetDataAsync:  Saves character data to MongoDB.
    /// </summary>
    /// <param name="characterDto"></param>
    /// <returns></returns>
    public async Task<bool> SetDataAsync(CharacterDto characterDto)
    {
        var filter = Builders<CharacterDto>.Filter.Eq(c => c.Name, characterDto.Name);
        var updateOptions = new UpdateOptions { IsUpsert = true };

        var update = Builders<CharacterDto>.Update
            .Set(c => c.Name, characterDto.Name)
            .Set(c => c.Films, characterDto.Films)
            .Set(c => c.Vehicles, characterDto.Vehicles);

        var result = await _charactersCollection.UpdateOneAsync(filter, update, updateOptions);
        return result.ModifiedCount > 0 || result.UpsertedId != null;

    }
}
