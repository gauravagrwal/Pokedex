namespace Pokédex.Models;

public class Pokémons
{
    public string Name { get; set; }
    public string Image => $"https://img.pokemondb.net/artwork/{Name}.jpg";
}
