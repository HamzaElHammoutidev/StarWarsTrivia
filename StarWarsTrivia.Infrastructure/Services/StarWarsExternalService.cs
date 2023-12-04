using StarWars.Application.Interfaces;
using StarWars.Domain.Entities;
using Newtonsoft.Json;
using StarWars.Application.DTOs;
using Microsoft.Extensions.Logging;
using Polly;

namespace StarWars.Infrastructure.Services;


/// <summary>
/// StarWarsExternalService : implementation of IStarWarsExternalService for a service to fetch data from an external Star Wars API.
/// </summary>
public class StarWarsExternalService : IStarWarsExternalService
{
    /// <summary>
    /// HttpCLient : D.I of HttpClient used to interact with external API.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// base url : Impvs. I can add this to settings file.
    /// </summary>
    private  string _baseApiUrl = "https://swapi.dev/api/";

    /// <summary>
    /// Logger : Serilog logger to keep log of any error or requests.
    /// </summary>
    private ILogger<StarWarsExternalService> _logger;

    /// <summary>
    /// Polly : D.I of polly used to improve resiliency by adding a retrying layer.
    /// </summary>
    private readonly IAsyncPolicy<HttpResponseMessage> _httpRetryPolicy;

    public StarWarsExternalService(HttpClient httpClient, ILogger<StarWarsExternalService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpRetryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .Or<HttpRequestException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (response, timespan, retryCount, context) =>
                {
                    _logger.LogError($"Retry {retryCount}. Waiting {timespan}. Reason: {response.Result?.ReasonPhrase ?? response.Exception?.Message}");
                });

    }

    /// <summary>
    /// FetchCharacterByNameAsync : fetches a list of characters using a name.
    /// </summary>
    /// <param name="name">Character name</param>
    /// <returns></returns>
    public async Task<List<Character>> FetchCharacterByNameAsync(string name)
    {
        var people = new List<Character>();
        var url = $"{_baseApiUrl}people/?search={name}";

        while (!string.IsNullOrEmpty(url))
        {

            HttpResponseMessage response = await _httpRetryPolicy.ExecuteAsync(() =>
                _httpClient.GetAsync(url));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error fetching character data from SWAPI {url}. Status Code: {response.StatusCode}. Reason: {response.ReasonPhrase}");

                throw new HttpRequestException($"Error fetching data from SWAPI: {response.ReasonPhrase}");
            }

            StarWarsServiceCharacterDto? result;
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<StarWarsServiceCharacterDto>(content);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing response from SWAPI.");

                throw new InvalidOperationException("Error deserializing response from SWAPI.", ex);
            }
            if (result is not null && result.Results is not null) 
                people.AddRange(result.Results);
            url = result!.Next;
        }

        return people;
    }

    /// <summary>
    /// FetchFilmDataAsync : fetches data for a single film using its URL.
    /// </summary>
    /// <param name="url">Film API URL</param>
    /// <returns></returns>
    public async Task<Film?> FetchFilmDataAsync(string url)
    {
        HttpResponseMessage response = await _httpRetryPolicy.ExecuteAsync(() =>
                 _httpClient.GetAsync(url));

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Error fetching film data from SWAPI {url}. Status Code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
            throw new HttpRequestException($"Error fetching film data: {response.ReasonPhrase}", null, response.StatusCode);
        }

        try
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Film>(content);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing Film data from SWAPI.");

            throw new InvalidOperationException("Error deserializing Film data from SWAPI.", ex);
        }

    }

    /// <summary>
    /// FetchVehicleDataAsync : fetches data for a single vehicle using its URL.
    /// </summary>
    /// <param name="url">Vehi
    public async Task<Vehicle?> FetchVehicleDataAsync(string url)
    {
       HttpResponseMessage response = await _httpRetryPolicy.ExecuteAsync(() => _httpClient.GetAsync(url));
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Error fetching vehicle data from SWAPI {url}. Status Code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
            throw new HttpRequestException($"Error fetching vehicle data: {response.ReasonPhrase}", null, response.StatusCode);
        }

        try
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Vehicle>(content);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing vehicle data from SWAPI.");

            throw new InvalidOperationException("Error deserializing Vehicle data from SWAPI.", ex);
        }

    }

}
