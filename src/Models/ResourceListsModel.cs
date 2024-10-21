using Pokédex.Helpers;
using Realms;

namespace Pokédex.Models;

public class ResourceItemModel:EmbeddedObject
{
    public int Id => PokédexHelper.ExtractIdFromUrl(Url);
    public string Name { get; set; }
    public string Url { get; set; }
}

public partial class ResourceListsModel : IRealmObject
{
    public int Count { get; set; }
    public string Next { get; set; }
    public string Previous { get; set; }
    public IEnumerable<ResourceItemModel> Results { get; set; }
}
