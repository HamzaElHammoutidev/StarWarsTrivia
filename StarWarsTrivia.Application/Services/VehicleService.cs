using StarWars.Application.Interfaces;

namespace StarWars.Application.Services
{
    /// <summary>
    /// VehicleService : Implementation of IVehicleService for business operation related to Vehicle.
    /// </summary>
    public class VehicleService : IVehicleService
    {

        /// <summary>
        /// IStarWarsExternalService : D.I of external Star Wars API.
        /// </summary>
        private readonly IStarWarsExternalService _starWarsService;

        /// <summary>
        /// VehicleService : Constructor
        /// </summary>
        /// <param name="starWarsService"></param>
        public VehicleService(IStarWarsExternalService starWarsService)
        {
            _starWarsService = starWarsService;
        }

        /// <summary>
        /// FetchVehiclesAsync : Concurrently fetches vehicle data based on a collection of vehicle URLs.
        /// </summary>
        /// <param name="filmUrls"></param>
        /// <returns></returns>
        public async Task<List<string>> FetchVehiclesAsync(IEnumerable<string> VehicleUrls)
        {
            var VehicleTasks = VehicleUrls.Select(url => _starWarsService.FetchVehicleDataAsync(url));
            var Vehicles = await Task.WhenAll(VehicleTasks);
            return Vehicles.Where(Vehicle => Vehicle != null).Select(Vehicle => Vehicle!.Name).ToList()!;
        }

    }
}
