using API.Models;
using System.Linq;
using API.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("item")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService = new ItemService();
        private readonly CategoryService _categoryService = new CategoryService();
        private readonly ShoppingListService _shoppingListService = new ShoppingListService();

        [HttpGet]
        public async Task<List<ItemGroup>> GetItems()
        {
            var items = await _itemService.GetItems();

            var groupedItems = items.GroupBy(x => x.Category.Name, (key, group) => new ItemGroup
            {
                Name = key,
                Items = group
            }).OrderBy(x => x.Name).ToList();

            return groupedItems;
        }

        [HttpGet("{id}")]
        public async Task<Item> GetItem([FromRoute] int id) => await _itemService.GetItem(id);

        [HttpPost("new")]
        public async Task<ObjectResult> CreateItem([FromBody] Item item)
        {
            if (string.IsNullOrEmpty(item.Name))
                return BadRequest("Name cannot be empty");

            var categoryExists = await _categoryService.CheckIfCategoryExists(item.Category.Id);

            if (!categoryExists)
                return BadRequest("Invalid Category");

            await _itemService.InsertItem(item);
            return Created("Item/New", item);
        }

        [HttpDelete("{id}")]
        public async Task DeleteItem([FromRoute] int id)
        {
            var isItemInAnyList = await _shoppingListService.AnyListHasItem(id);

            if (isItemInAnyList) await _shoppingListService.RemoveItemFromAllLists(id);

            await _itemService.DeleteItem(id);
        }
    }
}
