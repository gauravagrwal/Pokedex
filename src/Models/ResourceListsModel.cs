using Pokédex.Helpers;

namespace Pokédex.Models;

public class ResourceItemModel
{
    public int Id => PokédexHelper.ExtractIdFromUrl(Url);
    public string Name { get; set; }
    public string Url { get; set; }
}

public class ResourceListsModel
{
    public int Count { get; set; }
    public string Next { get; set; }
    public string Previous { get; set; }
    public IEnumerable<Pokémons> Results { get; set; }
}
