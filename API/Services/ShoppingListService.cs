using System;
using API.Models;
using API.Models.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace API.Services
{
    public class ShoppingListService : BaseConnection
    {
        public async Task<List<ShoppingList>> GetShoppingLists()
        {
            var lists = new List<ShoppingList>();

            Cmd.CommandText = "select id, listName, listDate, isActive, status from list";

            Con.Open();

            var result = await Cmd.ExecuteReaderAsync();

            while (result.Read())
            {
                lists.Add(new ShoppingList
                {
                    Id = result.GetInt32(0),
                    Name = result.GetString(1),
                    Date = result.GetDateTime(2),
                    IsActive = result.GetBoolean(3),
                    Status = (Status)Enum.Parse(typeof(Status), result.GetString(4), true)
                });
            }

            Con.Close();

            return lists;
        }

        public async Task<ShoppingList> GetShoppingList(int listId)
        {
            ShoppingList shoppingList = null;
            Cmd.CommandText = "select list.id listId, listName, listDate from list where id = @listId";
            Cmd.Parameters.AddWithValue("@listId", listId);

            Con.Open();

            var result = await Cmd.ExecuteReaderAsync();

            if (result.Read())
            {
                shoppingList = new ShoppingList
                {
                    Id = result.GetInt32(0),
                    Name = result.GetString(1),
                    Date = result.GetDateTime(2),
                };
            }

            Con.Close();

            return shoppingList;
        }

        public async Task<ShoppingList> GetActiveList()
        {
            ShoppingList shoppingList = null;
            Cmd.CommandText = "select list.id listId, listName, listDate, isActive from list where isActive = 1";

            Con.Open();

            var result = await Cmd.ExecuteReaderAsync();

            if (result.Read())
            {
                shoppingList = new ShoppingList
                {
                    Id = result.GetInt32(0),
                    Name = result.GetString(1),
                    Date = result.GetDateTime(2),
                    IsActive = result.GetBoolean(3)
                };
            }

            Con.Close();

            return shoppingList;
        }

        public async Task<List<Item>> GetShoppingListItems(int listId)
        {
            var items = new List<Item>();
            Cmd.CommandText = "select item.id ItemId, item.name ItemName, note, imageurl, category.id CategoryId, category.name CategoryName, quantity, iscompleted from list_item inner join item on idItem = item.id inner join category on categoryId = category.id where idList = @list";
            Cmd.Parameters.AddWithValue("@list", listId);

            Con.Open();

            var result = await Cmd.ExecuteReaderAsync();

            while (await result.ReadAsync())
            {
                items.Add(new Item
                {
                    Id = result.GetInt32(0),
                    Name = result.GetString(1),
                    Note = result.GetString(2),
                    ImageUrl = result.GetString(3),
                    Category = new Category
                    {
                        Id = result.GetInt32(4),
                        Name = result.GetString(5),
                    },
                    Quantity = result.GetInt32(6),
                    IsCompleted = result.GetBoolean(7)
                });
            }

            Con.Close();

            return items;
        }

        public async Task UpdateShoppingListStatus(int idList, Status status)
        {
            try
            {
                Cmd.CommandText = "update list set status = @status, isActive = 0 where id = @id";
                Cmd.Parameters.AddWithValue("@status", status.ToString().ToLower());
                Cmd.Parameters.AddWithValue("@id", idList);

                Con.Open();

                await Cmd.ExecuteNonQueryAsync();

                Con.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> CheckIfListHasItems(int idList)
        {
            Cmd.CommandText = "select count(*) from list_item where idList = @idList";
            Cmd.Parameters.AddWithValue("@idList", idList);

            Con.Open();

            var data = await Cmd.ExecuteReaderAsync();
            var hasRows = data.HasRows;

            Con.Close();

            return hasRows;
        }

        public async Task RemoveAllItemsFromList(int idList)
        {
            Cmd.CommandText = "delete from list_item where idList = @idList";
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@idList", idList);

            Con.Open();

            await Cmd.ExecuteNonQueryAsync();

            Con.Close();
        }

        public async Task<bool> AnyListHasItem(int itemId)
        {
            Cmd.CommandText = "select idList from list_item where idItem = @idItem";
            Cmd.Parameters.AddWithValue("@idItem", itemId);

            Con.Open();

            var data = await Cmd.ExecuteReaderAsync();
            var hasRows = data.HasRows;

            Con.Close();

            return hasRows;
        }

        public async Task UpdateItemStatus(int idList, int idItem, bool isCompleted)
        {
            Cmd.CommandText = "update list_item set iscompleted = @isCompleted where idList = @idList and idItem = @idItem";
            Cmd.Parameters.AddWithValue("@idList", idList);
            Cmd.Parameters.AddWithValue("@idItem", idItem);
            Cmd.Parameters.AddWithValue("@isCompleted", isCompleted ? 1 : 0);

            Con.Open();

            await Cmd.ExecuteNonQueryAsync();

            Con.Close();
        }

        public async Task RemoveItemFromAllLists(int itemId)
        {
            Cmd.CommandText = "delete from list_item where idItem = @itemId";
            Cmd.Parameters.Clear();
            Cmd.Parameters.AddWithValue("@itemId", itemId);

            Con.Open();

            await Cmd.ExecuteNonQueryAsync();

            Con.Close();
        }

        public async Task<ShoppingList> CreateShoppingList(ShoppingList list)
        {
            Cmd.CommandText = "insert into list(listName, listDate) values(@name, @date)";
            Cmd.Parameters.AddWithValue("@name", list.Name);
            Cmd.Parameters.AddWithValue("@date", list.Date);

            Con.Open();

            var result = await Cmd.ExecuteNonQueryAsync();

            Con.Close();

            if (result != 1)
                throw new Exception("Unexpected error");

            return list;
        }

        public async Task UpdateShoppingList(string name, int listId)
        {
            Cmd.CommandText = "update list set listName = @name where id = @id";
            Cmd.Parameters.AddWithValue("@name", name);
            Cmd.Parameters.AddWithValue("@id", listId);

            Con.Open();

            var result = await Cmd.ExecuteNonQueryAsync();

            Con.Close();

            if (result <= 0)
                throw new Exception("Unexpected error");
        }

        public async Task SetListAsActive(int listId)
        {
            Cmd.CommandText = "update list set isActive = 0";

            Con.Open();

            var result = await Cmd.ExecuteNonQueryAsync();

            if (result < 1) throw new Exception("Unexpected error");

            Cmd.CommandText = "update list set isActive = 1 where id = @idList";
            Cmd.Parameters.AddWithValue("@idList", listId);

            result = await Cmd.ExecuteNonQueryAsync();

            if (result != 1) throw new Exception("Unexpected error");

            Con.Close();
        }

        public async Task DeleteShoppingList(int id)
        {
            Cmd.CommandText = "delete from list where id = @id";
            Cmd.Parameters.AddWithValue("@id", id);

            Con.Open();

            var result = await Cmd.ExecuteNonQueryAsync();

            Con.Close();

            if (result != 1)                
            throw new Exception("Unexpected error");
        }

        public async Task<bool> CancelShoppingList(int id)
        {
            Cmd.CommandText = "update list set isActive = 0 where id = @idList";
            Cmd.Parameters.AddWithValue("@idList", id);

            Con.Open();

            var result = await Cmd.ExecuteNonQueryAsync();

            Con.Close();

            if (result != 0)
                return true;

            throw new Exception("Unexpected error");
        }

        public async Task AddItemToList(int idList, Item item)
        {
            Cmd.CommandText = "select * from list_item where idList = @idList and idItem = @idItem";
            Cmd.Parameters.AddWithValue("@idList", idList);
            Cmd.Parameters.AddWithValue("@idItem", item.Id);

            Con.Open();

            var data = await Cmd.ExecuteReaderAsync();

            if (data.HasRows)
                throw new Exception("Item already in the list");

            data.Close();

            Cmd.CommandText = "insert into list_item(idList, idItem, quantity) values(@idList, @idItem, @quantity)";
            Cmd.Parameters.AddWithValue("@quantity", item.Quantity);

            await Cmd.ExecuteNonQueryAsync();

            Con.Close();
        }

        public async Task RemoveItemFromList(int idList, int idItem)
        {
            Cmd.CommandText = "delete from list_item where idList = @idList and idItem = @idItem";
            Cmd.Parameters.AddWithValue("@idList", idList);
            Cmd.Parameters.AddWithValue("@idItem", idItem);

            Con.Open();

            var result = await Cmd.ExecuteNonQueryAsync();

            Con.Close();

            if (result != 1)
                throw new Exception("Unexpected error");
        }

        public async Task UpdateItemQuantity(int idList, int idItem, int quantity)
        {
            Cmd.CommandText = "update list_item set quantity = @quantity where idList = @idList and idItem = @idItem";
            Cmd.Parameters.AddWithValue("@quantity", quantity);
            Cmd.Parameters.AddWithValue("@idList", idList);
            Cmd.Parameters.AddWithValue("@idItem", idItem);

            Con.Open();

            var result = await Cmd.ExecuteNonQueryAsync();

            Con.Close();

            if (result <= 0)
                throw new Exception("Unexpected error");

        }
    }
}
