using Pokédex.Models;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pokédex.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties
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

        public ObservableCollection<PokémonModel> PokémonCollection { get; set; }
        #endregion

        public ICommand LoadMoreCommand { get; }
        public ICommand SearchCommand { get; }

        public MainViewModel()
        {
            IsLoading = false;
            IsVisible = false;
            PokémonCollection = new ObservableCollection<PokémonModel>();

            GetPokémons();

            LoadMoreCommand = new Command(async () => await LoadMoreCommandHandler());
            SearchCommand = new Command(async () => await SearchCommandHandler());

        }

        async Task LoadMoreCommandHandler()
        {
            IsLoading = true;
            IsVisible = true;
            int value = PokémonCollection.Count + 20;

            var pokémonList = new ObservableCollection<PokémonModel>();

            await Task.Run(() =>
            {
                pokémonList = GetPokémonsWithLimit(value);
                PokémonCollection.Clear();
                foreach (var pokemon in pokémonList)
                {
                    PokémonCollection.Add(pokemon);
                }
            });
            IsLoading = false;
            IsVisible = false;
        }

        async Task SearchCommandHandler()
        {
            await App.Current.MainPage.Navigation.PushAsync(new Pages.SearchPage());
        }

        public void GetPokémons()
        {
            PokémonCollection = GetPokémonsWithLimit();
        }

        public ObservableCollection<PokémonModel> GetPokémonsWithLimit(int value = 10)
        {
            var url = $"https://pokeapi.co/api/v2/pokemon/?offset=0&limit={value}";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = httpClient.GetStringAsync(url).Result;
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var jsonResponse = JsonSerializer.Deserialize<PaginationModel>(response, options);
                    return new ObservableCollection<PokémonModel>(jsonResponse.Results);
                }
                catch (Exception ex)
                {
                    IsLoading = false;
                    Console.WriteLine(ex.Message);
                    return new ObservableCollection<PokémonModel>();
                }
            }
        }
    }
}
