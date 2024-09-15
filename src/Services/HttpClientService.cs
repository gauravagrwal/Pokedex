using System.Net.Http.Json;
using System.Text.Json;
using Pokédex.Models;

namespace Pokédex.Service;

public class HttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public HttpClientService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://pokeapi.co/api/v2/")
        };

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = true,
        };
    }

    public IList<Pokémons> GetPokémons(string requestUri)
    {
        var pokémons = new List<Pokémons>();
        try
        {
            var response = _httpClient.GetAsync(requestUri).Result;
            var json = response.Content.ReadFromJsonAsync<PaginationModel>().Result;
            foreach (var result in json.Results)
            {
                pokémons.Add(result);
            }
        }
        catch (Exception ex)
        {
        }
        return pokémons;
    }

    public IList<Pokémons> GetPokémon(string requestUri)
    {
        var pokémons = new List<Pokémons>();
        try
        {
            var response = _httpClient.GetAsync(requestUri).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var jsonResponse = JsonSerializer.Deserialize<PaginationModel>(json, _jsonSerializerOptions);
            foreach (var result in jsonResponse.Results)
            {
                pokémons.Add(result);
            }
        }
        catch (Exception ex)
        {
        }
        return pokémons;
    }
}
