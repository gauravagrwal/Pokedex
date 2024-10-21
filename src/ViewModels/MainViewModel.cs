using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Pokédex.Models;
using Pokédex.Services;

namespace Pokédex.ViewModels;

public class MainViewModel
{
    private readonly HttpClientService _httpClientService;

    public bool IsLoading { get; set; }
    public bool IsVisible { get; set; }

    public int Offset { get; set; }

    public int Limit => Offset + Constants.LIMIT;

    public ObservableCollection<Pokémons> Pokémons { get; set; }

    public IRelayCommand LoadMoreCommand { get; }
    public IRelayCommand SearchCommand { get; }

    public MainViewModel(HttpClientService httpClientService)
    {
        _httpClientService = httpClientService;

        IsLoading = false;
        IsVisible = false;
        Pokémons = new ObservableCollection<Pokémons>();

        GetPokémons();

        LoadMoreCommand = new RelayCommand(async () => await LoadMoreCommandHandler());
        SearchCommand = new RelayCommand(async () => await SearchCommandHandler());
    }

    private async Task LoadMoreCommandHandler()
    {
        IsLoading = true;
        IsVisible = true;

        var pokémonList = new ObservableCollection<Pokémons>();

        await Task.Run(() =>
        {
            Pokémons = GetPokémonsWithLimit();
            Pokémons.Clear();
            foreach (var pokemon in pokémonList)
            {
                Pokémons.Add(pokemon);
            }
        });
        IsLoading = false;
        IsVisible = false;
    }

    private async Task SearchCommandHandler()
    {
        //await App.Current.MainPage.Navigation.PushAsync(new Views.SearchPage());
    }

    private void GetPokémons()
    {
        Pokémons = GetPokémonsWithLimit();
    }

    private ObservableCollection<Pokémons> GetPokémonsWithLimit()
    {
        try
        {
            var pokémons = _httpClientService.GetPokémons($"pokemon/?offset={Offset}&limit={Limit}");
            return new ObservableCollection<Pokémons>(pokémons);
        }
        catch (Exception ex)
        {
            IsLoading = false;
            Console.WriteLine(ex.Message);
            return new ObservableCollection<Pokémons>();
        }
    }
}
