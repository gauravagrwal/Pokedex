using System.Text.Json;
using System.Windows.Input;
using Pokédex.Models;

namespace Pokédex.ViewModels;

internal class SearchViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Image { get; set; }
    public string FilterPokemon { get; set; }

    public ICommand SearchPokemonCommand { get; }

    public SearchViewModel()
    {
        SearchPokemonCommand = new Command(SearchPokemonCommandHandler);
    }

    private async void SearchPokemonCommandHandler()
    {
        if (string.IsNullOrEmpty(FilterPokemon))
        {
            FilterPokemon = string.Empty;
        }

        string pokemon = FilterPokemon.Trim().ToLower();

        var url = "https://pokeapi.co/api/v2/pokemon";

        using (var httpClient = new HttpClient())
        {
            try
            {
                var response = await httpClient.GetAsync($"{url}/{pokemon}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var pokémon = JsonSerializer.Deserialize<Pokémon>(json);

                    Id = pokémon.Id;
                    Name = pokémon.Name;
                    Type = pokémon.Types[0].Type.Name;
                    Image = pokémon.Sprites.Other.OfficialArtwork.FrontDefault.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
