using System.Collections.Generic;

namespace Pokédex.Models
{
    public class PokémonModel
    {
        public string Name { get; set; }
        public string Image => $"https://img.pokemondb.net/artwork/{Name}.jpg";
    }

    public class PaginationModel
    {
        public int Count { get; set; }
        public string Previous { get; set; }
        public string Next { get; set; }
        public IList<PokémonModel> Results { get; set; }
    }
}
