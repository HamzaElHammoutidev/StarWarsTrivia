using StarWars.Application.Interfaces;

namespace StarWars.Api.GraphQL
{
    /// <summary>
    /// QueryType : Defines GraphQL query structure of the API.
    /// </summary>
    public class QueryType : ObjectType
    {

        /// <summary>
        /// Configure : Configure the structure of the query for the API and redirect to specific business service.
        /// </summary>
        /// <param name="descriptor"></param>
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Query");
            descriptor.Field("characters")
                .Type<ListType<CharacterType>>()
                .Argument("searchTerm", a => a.Type<StringType>())
                .Resolve(context =>
                {
                    var searchTerm = context.ArgumentValue<string>("searchTerm");
                    var characterResolver = context.Service<ICharacterService>();
                    return characterResolver.SearchcharacterAsync(searchTerm);
                });
        }
    }
}
