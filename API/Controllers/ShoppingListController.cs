using API.Models;
using API.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models.Enums;

namespace API.Controllers
{
    [Route("shoplist")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly ShoppingListService _shoppingListService = new ShoppingListService();

        [HttpGet]
        public async Task<dynamic> GetAllLists(bool grouped = false)
        {
            var lists = await _shoppingListService.GetShoppingLists();

            if (grouped)
            {
                return lists.GroupBy(x => new { x.Date.Month, x.Date.Year }, (key, group) => new ShoppingListGroup
                {
                    Date = new DateTime(key.Year, key.Month, 1),
                    ShoppingLists = group
                }).ToList().OrderBy(x => x.Date);
            }

            return lists;
        }

        [HttpGet("active")]
        public async Task<ShoppingList> GetActiveList()
        {
            var shoppinglist = await _shoppingListService.GetActiveList();

            if (shoppinglist is null) return shoppinglist;

            var shoppingListItems = await _shoppingListService.GetShoppingListItems(shoppinglist.Id);

            var groupedItems = shoppingListItems.GroupBy(x => x.Category.Name, (key, group) => new ItemGroup
            {
                Name = key,
                Items = group
            }).ToList();

            foreach (var item in groupedItems)
                shoppinglist.Groups.Add(item);

            return shoppinglist;
        }

        [HttpGet("{listId}")]
        public async Task<ShoppingList> GetList([FromRoute] int listId)
        {
            var shoppinglist = await _shoppingListService.GetShoppingList(listId);

            var shoppingListItems = await _shoppingListService.GetShoppingListItems(listId);

            var groupedItems = shoppingListItems.GroupBy(x => x.Category.Name, (key, group) => new ItemGroup
            {
                Name = key,
                Items = group
            }).ToList();

            foreach (var item in groupedItems)
                shoppinglist.Groups.Add(item);

            return shoppinglist;
        }

        [HttpPost("new")]
        public async Task<ObjectResult> CreateShoppingList([FromBody] ShoppingList list)
        {
            if (string.IsNullOrEmpty(list.Name.Trim()) || list.Name.Length < 3)
                return BadRequest("List must have a name and it must be greater than 2 characteres");

            if (list.Date < DateTime.Today)
                return BadRequest("List cannot have a date smaller than today");

            await _shoppingListService.CreateShoppingList(list);

            return Created("shoplist/new", list);
        }

        [HttpPut("{idList}")]
        public async Task UpdateShoppingList([FromRoute] int idList, [FromQuery] string name) => await _shoppingListService.UpdateShoppingList(name, idList);

        [HttpPatch("{idList}/{status}")]
        public async Task<ObjectResult> UpdateListStatus([FromRoute] int idList, [FromRoute] Status status)
        {
            try
            {
                await _shoppingListService.UpdateShoppingListStatus(idList, status);
                return StatusCode(204, "List successfully updated");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("active/{idList}")]
        public async Task SetListAsActive([FromRoute] int idList) => await _shoppingListService.SetListAsActive(idList);

        [HttpDelete("{idList}")]
        public async Task DeleteList([FromRoute] int idList)
        {
            var listHasItems = await _shoppingListService.CheckIfListHasItems(idList);

            if (listHasItems) await _shoppingListService.RemoveAllItemsFromList(idList);

            await _shoppingListService.DeleteShoppingList(idList);
        }

        #region Shopping list items

        [HttpPost("{idList}/add")]
        public async Task<ObjectResult> AddItemToList([FromBody] Item item, [FromRoute] int idList)
        {
            try
            {
                await _shoppingListService.AddItemToList(idList, item);
                return Created("AddItemToList", item);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpDelete("{idList}/{idItem}")]
        public async Task RemoveItemFromLiist([FromRoute] int idList, [FromRoute] int idItem) => await _shoppingListService.RemoveItemFromList(idList, idItem);

        [HttpPut("{idList}/{idItem}")]
        public async Task UpdateItemQuantity([FromRoute] int idList, [FromRoute] int idItem, [FromQuery] int quantity) => await _shoppingListService.UpdateItemQuantity(idList, idItem, quantity);

        [HttpPut("{idList}/{idItem}/{isCompleted}")]
        public async Task UpdateItemStatus([FromRoute] int idList, [FromRoute] int idItem, [FromRoute] bool IsCompleted) => await _shoppingListService.UpdateItemStatus(idList, idItem, IsCompleted);

        #endregion
    }
}
