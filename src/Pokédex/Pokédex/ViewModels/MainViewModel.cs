using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Pokédex.Models;
using Xamarin.Forms;

namespace Pokédex.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

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

        public ObservableCollection<Pokémons> PokémonCollection { get; set; }
        #endregion

        public ICommand LoadMoreCommand { get; }

        public MainViewModel()
        {
            _jsonSerializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            IsLoading = false;
            IsVisible = false;
            PokémonCollection = new ObservableCollection<Pokémons>();

            GetPokémons();

            LoadMoreCommand = new Command(async () => await LoadMoreCommandHandler());
        }

        async Task LoadMoreCommandHandler()
        {
            IsLoading = true;
            IsVisible = true;
            int value = PokémonCollection.Count + 20;

            var pokémonList = new ObservableCollection<Pokémons>();

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

        public void GetPokémons()
        {
            PokémonCollection = GetPokémonsWithLimit();
        }

        public ObservableCollection<Pokémons> GetPokémonsWithLimit(int value = 10)
        {
            var url = $"https://pokeapi.co/api/v2/pokemon/?offset=0&limit={value}";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = httpClient.GetStringAsync(url).Result;
                    var jsonResponse = JsonSerializer.Deserialize<PaginationModel>(response, _jsonSerializerOptions);
                    return new ObservableCollection<Pokémons>(jsonResponse.Results);
                }
                catch (Exception ex)
                {
                    IsLoading = false;
                    Console.WriteLine(ex.Message);
                    return new ObservableCollection<Pokémons>();
                }
            }
        }
    }
}
