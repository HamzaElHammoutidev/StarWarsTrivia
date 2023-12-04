using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL;
using StarWars.Application.DTOs;
using Microsoft.Extensions.Options;
using StarWars.Infrastructure.Helpers;
using StarWarsUI.Models;
using Newtonsoft.Json;

namespace StarWarsUI.Data
{
    public class CharacterService
    {
        private readonly GraphQLHttpClient _client;


        public CharacterService(HttpClient httpClient, IOptions<GraphQLSettings> graphqlSettings)
        {
            var options = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(graphqlSettings.Value.ApiUrl!) ?? throw new Exception("Error fetching ApiUrl"), 
            };

            _client = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer(), httpClient);
        }

        public async Task<List<CharacterDto>> GetCharactersAsync(string searchTerm)
        {
            try
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                        query MyQuery($searchTerm: String!) {
                            characters(searchTerm: $searchTerm) {
                                name
                                films
                                vehicles
                            }
                        }",
                    Variables = new { searchTerm }
                };

                var response = await _client.SendQueryAsync<CharactersResponse>(request);
                if (response is not null && response.Data is not null && response.Data.Characters is not null)
                {
                    return response!.Data!.Characters;
                }
                return new List<CharacterDto>();
            }
            catch (GraphQLHttpRequestException httpEx)
            {
                throw new ApplicationException("Error fetching data.", httpEx);
            }
            catch (JsonException jsonEx)
            {
                throw new ApplicationException("Error parsing data.", jsonEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error.", ex);
            }
        }
    }


}
