namespace Pokédex.Models;

public class Pokémons
{
    public string Name { get; set; }
    public string Image => $"https://img.pokemondb.net/artwork/{Name}.jpg";
}

public class PaginationModel
{
    public int Count { get; set; }
    public string Previous { get; set; }
    public string Next { get; set; }
    public IList<Pokémons> Results { get; set; }
}
