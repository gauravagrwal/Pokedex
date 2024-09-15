using System.Collections.ObjectModel;
using System.Windows.Input;
using Pokédex.Models;
using Pokédex.Service;

namespace Pokédex.ViewModels;

public class MainViewModel
{
    private readonly HttpClientService _httpClientService;

    public bool IsLoading { get; set; }
    public bool IsVisible { get; set; }

    public ObservableCollection<Pokémons> PokémonCollection { get; set; }

    public ICommand LoadMoreCommand { get; }
    public ICommand SearchCommand { get; }

    public MainViewModel(HttpClientService httpClientService)
    {
        _httpClientService = httpClientService;

        IsLoading = false;
        IsVisible = false;
        PokémonCollection = new ObservableCollection<Pokémons>();

        GetPokémons();

        LoadMoreCommand = new Command(async () => await LoadMoreCommandHandler());
        SearchCommand = new Command(async () => await SearchCommandHandler());
    }

    async Task LoadMoreCommandHandler()
    {
        IsLoading = true;
        IsVisible = true;
        int value = PokémonCollection.Count + 20;

        var pokémonList = new ObservableCollection<Pokémons>();

        await Task.Run(() =>
        {
            GetPokémonsWithLimit(value);
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
        await App.Current.MainPage.Navigation.PushAsync(new Views.SearchPage());
    }

    public void GetPokémons()
    {
        GetPokémonsWithLimit();
    }

    public void GetPokémonsWithLimit(int value = 20)
    {
        {
            try
            {
                var pokémons = _httpClientService.GetPokémons($"pokemon/?offset=0&limit={value}");
                PokémonCollection = new ObservableCollection<Pokémons>(pokémons);
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Console.WriteLine(ex.Message);
                PokémonCollection = new ObservableCollection<Pokémons>();
            }
        }
    }
}
