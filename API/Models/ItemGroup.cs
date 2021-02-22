using System.Collections.Generic;

namespace API.Models
{
    public class ItemGroup
    {
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
