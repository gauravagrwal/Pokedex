using System;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pokédex.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        #region Properties
        private string _id;
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _type;
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private string _image;
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private string _filterPokemon;
        public string FilterPokemon
        {
            get => _filterPokemon;
            set => SetProperty(ref _filterPokemon, value);
        }
        #endregion

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
                        var result = await response.Content.ReadAsStringAsync();

                        var jObject = JsonNode.Parse(result).AsObject();

                        Id = jObject["id"].ToString();
                        Name = jObject["name"].ToString();
                        Type = jObject["types"][0]["type"]["name"].ToString();
                        Image = jObject["sprites"]["other"]["official-artwork"]["front_default"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
