

using Amazon.Runtime.Internal.Util;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StarWars.Application.DTOs;
using StarWars.Application.Interfaces;
using StarWars.Domain.Entities;
namespace StarWars.Application.Services;


/// <summary>
/// CharacterService : Implementation of ICharacterService for business operation related to Character.
/// </summary>
public class CharacterService : ICharacterService
{
    /// <summary>
    /// IStarWarsExternalService : D.I of external Star Wars API.
    /// </summary>
    private readonly IStarWarsExternalService _starWarsService;

    /// <summary>
    /// IMongoDbService : D.I of Mongo Db Service.
    /// </summary>
    private readonly IMongoDbService _mongoService;

    /// <summary>
    /// ILogger<CharacterService> : D.I of Serilog for logging.
    /// </summary>
    private readonly ILogger<CharacterService> _logger;

    /// <summary>
    /// IFilmService : D.I of Film Service.
    /// </summary>
    private readonly IFilmService _filmService;

    /// <summary>
    /// IVehicleService : D.I of Vehicle Service.
    /// </summary>
    private readonly IVehicleService _vehicleService;


    /// <summary>
    /// CharacterService : Constructor
    /// </summary>
    /// <param name="starWarsService"></param>
    /// <param name="vehicleService"></param>
    /// <param name="filmService"></param>
    /// <param name="mongoService"></param>
    /// <param name="logger"></param>
    public CharacterService(IStarWarsExternalService starWarsService, IVehicleService vehicleService , IFilmService filmService, IMongoDbService mongoService,ILogger<CharacterService> logger)
    {
        _starWarsService = starWarsService;
        _mongoService = mongoService;
        _logger = logger;
        _filmService = filmService;
        _vehicleService = vehicleService;

    }

    /// <summary>
    /// SearchcharacterAsync : search for characters matching a specific name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<List<CharacterDto>> SearchcharacterAsync(string name)
    {
        var charactersDTO = new List<CharacterDto>();

        // Check Mongo first.
        var dbcharacters = await _mongoService.GetDataAsync(name);
        if (dbcharacters is not null && dbcharacters.Count > 0)
        {
            _logger.LogInformation($"Data fetched from MongoDB for {name}");

            foreach (var dbcharacter in dbcharacters)
            {
                charactersDTO.Add(dbcharacter);
            }
        }
        else
        {
            _logger.LogInformation($"Fetching data from SWAPI for {name}");

            // Fetch from SWAPI.
            var characters = await _starWarsService.FetchCharacterByNameAsync(name);
            if (characters is not null && characters.Count > 0 ) {
                
                foreach(var character in characters)
                {
                    var CharacterDto = new CharacterDto
                    {
                        Name = character.Name,
                        Films = new List<string>(),
                        Vehicles = new List<string>()
                    };

                    CharacterDto.Films = await _filmService.FetchFilmsAsync(character.Films!);
                    CharacterDto.Vehicles = await _vehicleService.FetchVehiclesAsync(character.Vehicles!);

                    bool isPersisted = await _mongoService.SetDataAsync(CharacterDto);
                    if (isPersisted)
                    {
                        _logger.LogInformation($"Data for {name} successfully saved in MondoDB");
                    }
                    else
                    {
                        _logger.LogError($"Failed to save data for {name} in MongoDB");
                    }
                    charactersDTO.Add(CharacterDto);
                }
            }
    }
        return charactersDTO;

    }





}
