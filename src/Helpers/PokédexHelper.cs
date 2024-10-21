namespace Pokédex.Helpers;

internal static class PokédexHelper
{
    internal static int ExtractIdFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return default;

        if (url.EndsWith("/"))
            url = url.Remove(url.Length - 1);

        var splitUrl = url.Split("/");

        if (int.TryParse(splitUrl[splitUrl.Count() - 1], out int id))
            return id;

        return default;
    }
}
