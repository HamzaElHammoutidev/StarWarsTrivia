using StarWars.Application.DTOs;

namespace StarWars.Application.Interfaces;


/// <summary>
/// IMongoDbService : Interface for a service that interacts with MongoDB.
/// </summary>
public interface IMongoDbService
{
    /// <summary>
    /// SetDataAsync:  Saves character data to MongoDB.
    /// </summary>
    /// <param name="characterDto"></param>
    /// <returns></returns>
    Task<bool> SetDataAsync(CharacterDto characterDto);


    /// <summary>
    /// GetDataAsync : retrieves character data based on a name.
    /// </summary>
    /// <param name="key">name of character</param>
    /// <returns></returns>
    Task<List<CharacterDto>> GetDataAsync(string key);
}