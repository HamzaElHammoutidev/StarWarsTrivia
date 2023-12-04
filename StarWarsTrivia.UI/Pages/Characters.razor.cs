
using Microsoft.AspNetCore.Components;
using StarWars.Application.DTOs;
using StarWarsUI.Data; 

namespace StarWarsUI.Pages 
{
    partial class Characters
    {
        [Inject]
        public CharacterService CharacterService { get; set; } = default!;

        public List<CharacterDto> characters { get; private set; } = new List<CharacterDto>();
        public bool IsLoading { get; private set; } = false;


        private string? searchTerm;
        private string? errorMessage;

        private async Task SearchCharacters()
        {
            IsLoading = true;
            errorMessage = null;
            try
            {
                characters = new();
                characters = await CharacterService.GetCharactersAsync(searchTerm!); 
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
        
        
}

