
using Newtonsoft.Json;

namespace StarWars.Domain.Entities
{

    /// <summary>
    /// Character : Character Domain Representation
    /// </summary>
    public class Character
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("height")]
        public string? Height { get; set; }

        [JsonProperty("mass")]
        public string? Mass { get; set; }

        [JsonProperty("hair_color")]
        public string? HairColor { get; set; }

        [JsonProperty("skin_color")]
        public string? SkinColor { get; set; }

        [JsonProperty("eye_color")]
        public string? EyeColor { get; set; }

        [JsonProperty("birth_year")]
        public string? BirthYear { get; set; }

        [JsonProperty("gender")]
        public string? Gender { get; set; }

        [JsonProperty("homeworld")]
        public string? Homeworld { get; set; }

        [JsonProperty("films")]
        public List<string>? Films { get; set; }

        [JsonProperty("species")]
        public List<string>? Species { get; set; }

        [JsonProperty("vehicles")]
        public List<string>? Vehicles { get; set; }

        [JsonProperty("starships")]
        public List<string>? Starships { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("edited")]
        public DateTime Edited { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }

        public override string ToString()
        {
            var filmsStr = string.Join(", ", Films!);
            var vehiclesStr = string.Join(", ", Vehicles!);
            var starshipsStr = string.Join(", ", Starships!);
            var speciesStr = string.Join(", ", Species!);

            return $"Name: {Name}, Height: {Height}, Mass: {Mass}, Hair Color: {HairColor}, " +
                   $"Skin Color: {SkinColor}, Eye Color: {EyeColor}, Birth Year: {BirthYear}, " +
                   $"Gender: {Gender}, Homeworld: {Homeworld}, Films: [{filmsStr}], " +
                   $"Species: [{speciesStr}], Vehicles: [{vehiclesStr}], Starships: [{starshipsStr}], " +
                   $"Created: {Created}, Edited: {Edited}, URL: {Url}";
        }
    }

}