using System.Net.Http.Json;
using System.Text.Json;

namespace Pokédex.Services;

public class HttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public HttpClientService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(Constants.BASE_URL)
        };

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = true,
        };
    }

    public async Task<T> GetResourceAsync<T>(string requestUri)
    {
        try
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<T>();
            return json;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return default;
        }
    }
}
