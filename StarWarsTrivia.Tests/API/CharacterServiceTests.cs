using Xunit;
using Moq;
using StarWars.Application.Services;
using StarWars.Application.Interfaces;
using Microsoft.Extensions.Logging;
using StarWars.Domain.Entities;
using StarWars.Application.DTOs;

namespace StarWars.Tests {

    /// <summary>
    /// CharacterServiceTests : Unit tests implmentation class
    /// </summary>
    public class CharacterServiceTests
    {

        /// <summary>
        /// SearchCharacterAsync_WhenDarthMaul_ShouldReturnExpectedAPIData : Expected return data from External SWAPI API.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchCharacterAsync_WhenDarthMaul_ShouldReturnExpectedAPIData()
        {
            // Arrange
            var mockStarWarsService = new Mock<IStarWarsExternalService>();
            var mockMongoService = new Mock<IMongoDbService>();
            var mockLogger = new Mock<ILogger<CharacterService>>();
            var mockFilmService = new Mock<IFilmService>();
            var mockVehicleService = new Mock<IVehicleService>();

            var darthMaul = new Character
            {
                Name = "Darth Maul",
                Films = new List<string> { "https://swapi.dev/api/films/4/" },
                Vehicles = new List<string> { "https://swapi.dev/api/vehicles/42/" } 
            };
            var phantomMenace = new Film { Title = "The Phantom Menace" };
            var speederBike = new Vehicle { Name = "FC-20 speeder bike" };


            mockStarWarsService.Setup(s => s.FetchCharacterByNameAsync("Darth Maul"))
                .ReturnsAsync(new List<Character> { darthMaul });
            mockFilmService.Setup(s => s.FetchFilmsAsync(new List<string> { "https://swapi.dev/api/films/4/" }))
                .ReturnsAsync(new List<string> { phantomMenace.Title});
            mockVehicleService.Setup(s => s.FetchVehiclesAsync(new List<string> { "https://swapi.dev/api/vehicles/42/" }))
                .ReturnsAsync(new List<string> { speederBike.Name});
            mockMongoService.Setup(c => c.GetDataAsync(It.IsAny<string>()))
                .ReturnsAsync( new List<CharacterDto>());

            var characterService = new CharacterService(mockStarWarsService.Object, mockVehicleService.Object, mockFilmService.Object, mockMongoService.Object, mockLogger.Object);

            // Act
            var result = await characterService.SearchcharacterAsync("Darth Maul");

            // Assert
            Assert.Single(result);
            var characterDto = result[0];
            Assert.Equal("Darth Maul", characterDto.Name);
            Assert.Contains(characterDto.Films!, f => f == "The Phantom Menace");
            Assert.Contains(characterDto.Vehicles!, v => v == "FC-20 speeder bike");
        }

        /// <summary>
        /// SearchCharacterAsync_WhenDarthMaulInDB_ShouldReturnDataFromDB : Expected data to be fetched from DB.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchCharacterAsync_WhenDarthMaulInDB_ShouldReturnDataFromDB()
        {
            // Arrange
            var mockStarWarsService = new Mock<IStarWarsExternalService>();
            var mockFilmService = new Mock<IFilmService>();
            var mockVehicleService = new Mock<IVehicleService>();
            var mockMongoService = new Mock<IMongoDbService>();
            var mockLogger = new Mock<ILogger<CharacterService>>();
            var persistedDarthMaul = new List<CharacterDto>
            {
                new CharacterDto
                {
                    Name = "Darth Maul",
                    Films = new List<string> { "The Phantom Menace" },
                    Vehicles = new List<string> { "FC-20 speeder bike" }
                }
            };

            mockMongoService.Setup(c => c.GetDataAsync("Darth Maul"))
                .ReturnsAsync(persistedDarthMaul);

            var characterService = new CharacterService(mockStarWarsService.Object,mockVehicleService.Object, mockFilmService.Object, mockMongoService.Object, mockLogger.Object);

            // Act
            var result = await characterService.SearchcharacterAsync("Darth Maul");

            // Assert
            Assert.Single(result);
            var characterDto = result[0];
            Assert.Equal("Darth Maul", characterDto.Name);
            Assert.Contains(characterDto.Films!, f => f == "The Phantom Menace");
            Assert.Contains(characterDto.Vehicles!, v => v == "FC-20 speeder bike");
        }

    }
}