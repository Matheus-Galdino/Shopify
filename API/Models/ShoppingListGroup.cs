using System;
using System.Collections.Generic;

namespace API.Models
{
    public class ShoppingListGroup
    {
        public DateTime Date { get; set; }
        public IEnumerable<ShoppingList> ShoppingLists { get; set; }
    }
}
