using Microsoft.Extensions.Logging;
using StarWars.Application.Interfaces;
using StarWars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Application.Services
{

    /// <summary>
    /// FilmService : Implementation of IFilmService for business operation related to Film.
    /// </summary>
    public class FilmService : IFilmService
    {

        /// <summary>
        /// IStarWarsExternalService : D.I of external Star Wars API.
        /// </summary>
        private readonly IStarWarsExternalService _starWarsService;

        /// <summary>
        /// Filmservice : Constructor
        /// </summary>
        /// <param name="starWarsService"></param>
        public FilmService(IStarWarsExternalService starWarsService)
        {
            _starWarsService = starWarsService;
        }

        /// <summary>
        /// FetchFilmsAsync : Concurrently fetches film data based on a collection of film URLs.
        /// </summary>
        /// <param name="filmUrls"></param>
        /// <returns></returns>
        public async Task<List<string>> FetchFilmsAsync(IEnumerable<string> filmUrls)
        {
            var filmTasks = filmUrls.Select(_starWarsService.FetchFilmDataAsync);
            var films = await Task.WhenAll(filmTasks);
            return films.Where(film => film != null).Select(film => film!.Title).ToList()!;
        }

    };
}
