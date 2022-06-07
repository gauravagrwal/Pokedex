using Newtonsoft.Json;
using Pokédex.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pokédex.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties
        public ObservableCollection<PokémonModel> PokémonCollection { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }
        #endregion

        #region Commands
        public ICommand LoadMoreCommand { get; }
        #endregion

        public MainViewModel()
        {
            IsLoading = false;
            PokémonCollection = new ObservableCollection<PokémonModel>();

            GetPokemons();

            LoadMoreCommand = new Command(LoadMoreCommandHandler);
        }

        #region Command Handlers
        async void LoadMoreCommandHandler()
        {
            IsLoading = true;
            IsVisible = true;
            int value = PokémonCollection.Count + 20;

            var pokémonlist = new ObservableCollection<PokémonModel>();

            await System.Threading.Tasks.Task.Run(() =>
            {
                pokémonlist = GetPokemonsWithLimit(value);
                PokémonCollection.Clear();

                foreach (var pokemon in pokémonlist)
                {
                    PokémonCollection.Add(pokemon);
                }
            });
            IsLoading = false;
            IsVisible = false;
        }
        #endregion

        #region Methods
        public void GetPokemons()
        {
            PokémonCollection = GetPokemonsWithLimit();
        }

        public ObservableCollection<PokémonModel> GetPokemonsWithLimit(int value = 10)
        {
            var url = "https://pokeapi.co/api/v2/pokemon/?offset=0&limit=" + value;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = httpClient.GetStringAsync(url).Result;
                    var jsonResponse = JsonConvert.DeserializeObject<PaginationModel>(response);
                    return new ObservableCollection<PokémonModel>(jsonResponse.Results);
                }
                catch (Exception ex)
                {
                    IsLoading = false;
                    Debug.WriteLine(ex.Message);
                    return new ObservableCollection<PokémonModel>();
                }
            }
        }
        #endregion
    }
}
