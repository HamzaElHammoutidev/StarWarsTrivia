using StarWars.Application.DTOs;

namespace StarWarsUI.Models
{
    public class CharactersResponse
    {

        public List<CharacterDto>? Characters { get; set; }
    }

    public class GraphQLDataResponse
    {
        public CharactersResponse? Data { get; set; }
    }
}
