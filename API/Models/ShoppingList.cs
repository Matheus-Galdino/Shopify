using System;
using API.Models.Enums;
using System.Collections.Generic;

namespace API.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public List<ItemGroup> Groups { get; private set; } = new List<ItemGroup>();

        public ShoppingList() { }

        public ShoppingList(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }
    }
}
