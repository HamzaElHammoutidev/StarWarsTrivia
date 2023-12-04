using StarWars.Application.DTOs;

namespace StarWars.Application.Interfaces;

/// <summary>
/// ICharacterService : Inferace for business operation related to Character.
/// </summary>
public interface ICharacterService
{
    /// <summary>
    /// SearchcharacterAsync : search for characters matching a specified name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<List<CharacterDto>> SearchcharacterAsync(string name);
}

