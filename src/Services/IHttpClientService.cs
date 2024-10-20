namespace Pokédex.Services;

public interface IHttpClientService
{
    Task<T> GetResourceAsync<T>(string requestUri);
}
