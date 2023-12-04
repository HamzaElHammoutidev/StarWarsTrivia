using StarWars.Domain.Entities;
namespace StarWars.Application.Interfaces
{
    /// <summary>
    /// IFilmService : Inferace for business operation related to Film.
    /// </summary>
    public interface IFilmService
    {
        /// <summary>
        /// FetchFilmsAsync : Fetches film data, based on a collection of film URLs.
        /// </summary>
        /// <param name="filmUrls"></param>
        /// <returns></returns>
        Task<List<string>> FetchFilmsAsync(IEnumerable<string> filmUrls);
    }
}
