using StarWars.Application.DTOs;

namespace StarWars.Api.GraphQL
{
    /// <summary>
    /// CharacterType : defines the GraphQL type for representation of Character.
    /// </summary>
    public class CharacterType : ObjectType<CharacterDto>
    {
        /// <summary>
        /// Configure : mapping between GraphQL fiels and their associated Character fields
        /// </summary>
        /// <param name="descriptor"></param>
        protected override void Configure(IObjectTypeDescriptor<CharacterDto> descriptor)
        {
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Films).Type<ListType<NonNullType<StringType>>>();
            descriptor.Field(c => c.Vehicles).Type<ListType<NonNullType<StringType>>>();
        }
    }
}
