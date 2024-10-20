using Pokédex.Service;

namespace Pokédex.ViewModels;

public class MainViewModel
{
    private readonly HttpClientService _httpClientService;

    public MainViewModel(HttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }
}
