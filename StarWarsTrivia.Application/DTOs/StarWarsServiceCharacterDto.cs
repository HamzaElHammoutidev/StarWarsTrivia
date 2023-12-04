using StarWars.Domain.Entities;

namespace StarWars.Application.DTOs
{

    /// <summary>
    /// StarWarsServiceCharacterDto : Data transfer object for Star Wars API result for Character.
    /// </summary>
    public class StarWarsServiceCharacterDto
    {
        /// <summary>
        /// Count.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Next.
        /// </summary>
        public string? Next { get; set; }

        /// <summary>
        /// Previous
        /// </summary>
        public string? Previous { get; set; }

        /// <summary>
        /// Results: List of Characters.
        /// </summary>
        public List<Character>? Results { get; set; }

        /// <summary>
        /// To String representation of StarWarsServiceCharacterDto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var resultsStr = Results != null
                             ? string.Join(", ", Results.Select(character => character.ToString()))
                             : "No results";

            return $"Count: {Count}, Next: {Next}, Previous: {Previous}, Results: [{resultsStr}]";
        }

    }
}
