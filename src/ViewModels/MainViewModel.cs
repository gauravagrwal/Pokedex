using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Pokédex.Models;
using Pokédex.Service;

namespace Pokédex.ViewModels;

public partial class MainViewModel
{
    private readonly HttpClientService _httpClientService;

    public bool IsLoading { get; set; }
    public bool IsVisible { get; set; }

    public ObservableCollection<Pokémons> PokémonCollection { get; set; }

    public MainViewModel(HttpClientService httpClientService)
    {
        _httpClientService = httpClientService;

        IsLoading = false;
        IsVisible = false;

        PokémonCollection = new ObservableCollection<Pokémons>();

        GetPokémons();
    }

    private void GetPokémons()
    {
        try
        {
            var pokémons = _httpClientService.GetPokémons($"pokemon");
            PokémonCollection = new ObservableCollection<Pokémons>(pokémons);
        }
        catch (Exception ex)
        {
            IsLoading = false;
            Console.WriteLine(ex.Message);
            PokémonCollection = new ObservableCollection<Pokémons>();
        }
    }

    [RelayCommand]
    private async Task Load()
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

    private void GetPokémonsWithLimit(int value = 20)
    {
        try
        {
            var pokémons = _httpClientService.GetPokémons($"pokemon/?offset={value}&limit={value}");
            PokémonCollection = new ObservableCollection<Pokémons>(pokémons);
        }
        catch (Exception ex)
        {
            IsLoading = false;
            Console.WriteLine(ex.Message);
            PokémonCollection = new ObservableCollection<Pokémons>();
        }
    }

    [RelayCommand]
    private async Task Search()
    {
        await App.Current.MainPage.Navigation.PushAsync(new Views.SearchPage());
    }
}
