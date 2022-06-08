using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pokédex.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;

        /* Commands */
        public ICommand SearchPokemonCommand { get; }

        /* Properties */
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

        public SearchViewModel()
        {
            SearchPokemonCommand = new Command(SearchPokemonCommandHandler);

            _httpClient = new HttpClient();
        }

        /* Command Handlers */
        private async void SearchPokemonCommandHandler()
        {
            if (string.IsNullOrEmpty(FilterPokemon))
            {
                FilterPokemon = string.Empty;
            }

            string pokemon = FilterPokemon.Trim().ToLower();

            try
            {
                string endpoint = "https://pokeapi.co/api/v2";
                string path = "pokemon";

                HttpResponseMessage response = await _httpClient.GetAsync($"{endpoint}/{path}/{pokemon}");

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(result);

                    Id = json["id"].ToString();
                    Name = json["name"].ToString();
                    Type = json["types"][0]["type"]["name"].ToString();
                    Image = json["sprites"]["other"]["official-artwork"]["front_default"].ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
