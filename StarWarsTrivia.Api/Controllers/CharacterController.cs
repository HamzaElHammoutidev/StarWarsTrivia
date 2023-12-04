using Microsoft.AspNetCore.Mvc;
using StarWars.Application.Interfaces;

namespace StarWars.Api.Controllers;



/// <summary>
/// Characters Controller : Restful HTTP API requests for characters.
/// </summary>
[ApiController]
[Route("[controller]")]
public class CharactersController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharactersController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    /// <summary>
    /// GetCharacterByName : Retrieve a character by their name.
    /// </summary>
    /// <param name="name">Character name</param>
    /// <returns>Character data or error</returns>
    [HttpGet("{name}")]
    public async Task<IActionResult> GetCharacterByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Invalid name");
        }

        var character = await _characterService.SearchcharacterAsync(name);
        if (character == null)
        {
            return NotFound();
        }
        return Ok(character);
    }
}
