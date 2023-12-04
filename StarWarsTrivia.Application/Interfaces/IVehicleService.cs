
namespace StarWars.Application.Interfaces
{
    /// <summary>
    /// IVehicleService : Inferace for business operation related to Vehicle.
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// FetchVehiclesAsync : Fetches film data based on a collection of vehicle URLs.
        /// </summary>
        /// <param name="vehiclUrls"></param>
        /// <returns></returns>
        Task<List<string>> FetchVehiclesAsync(IEnumerable<string> vehiclUrls);
    }
}
