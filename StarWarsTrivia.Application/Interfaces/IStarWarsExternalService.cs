using StarWars.Domain.Entities;
namespace StarWars.Application.Interfaces {


    /// <summary>
    /// IStarWarsExternalService : Interface for a service to fetch data from an external Star Wars API.
    /// </summary>
    public interface IStarWarsExternalService
    {
        /// <summary>
        /// FetchFilmDataAsync : fetches data for a single film using its URL.
        /// </summary>
        /// <param name="url">Film API URL</param>
        /// <returns></returns>
        Task<Film?> FetchFilmDataAsync(string url);

        /// <summary>
        /// FetchVehicleDataAsync : fetches data for a single vehicle using its URL.
        /// </summary>
        /// <param name="url">Vehicle API URL</param>
        /// <returns></returns>
        Task<Vehicle?> FetchVehicleDataAsync(string url);

        /// <summary>
        /// FetchCharacterByNameAsync : fetches a list of characters using a name.
        /// </summary>
        /// <param name="name">Character name</param>
        /// <returns></returns>
        Task<List<Character>> FetchCharacterByNameAsync(string name);
    }
}