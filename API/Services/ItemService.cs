using System;
using API.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace API.Services
{
    public class ItemService : BaseConnection
    {
        public async Task<Item> GetItem(int id)
        {
            Item queriedItem = null;
            Cmd.CommandText = "select item.id 'itemId', item.name 'itemName', note, imageUrl, category.id 'categoryId', category.name 'categoryName' from item inner join category on categoryId = category.id where item.id = @id";
            Cmd.Parameters.AddWithValue("@id", id);

            Con.Open();

            var result = await Cmd.ExecuteReaderAsync();

            if (!result.HasRows)
                throw new Exception($"Item with id {id} doesn't exist");

            if (result.Read())
            {
                queriedItem = new Item
                {
                    Id = result.GetInt32(0),
                    Name = result.GetString(1),
                    Note = result.GetString(2),
                    ImageUrl = result.GetString(3),
                    Category = new Category
                    {
                        Id = result.GetInt32(4),
                        Name = result.GetString(5),
                    }
                };
            }

            Con.Close();

            return queriedItem;
        }

        public async Task<List<Item>> GetItems()
        {
            var items = new List<Item>();
            Cmd.CommandText = "select item.id 'itemId', item.name 'itemName', note, imageUrl, category.id 'categoryId', category.name 'categoryName' from item inner join category on categoryId = category.id order by item.name";

            Con.Open();

            var result = await Cmd.ExecuteReaderAsync();

            while (result.Read())
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
                    }
                });
            }

            Con.Close();

            return items;
        }

        public async Task<Item> InsertItem(Item item)
        {
            Cmd.CommandText = "insert into item(name, note, imageUrl, categoryId) values(@name, @note, @imageUrl, @categoryId)";
            Cmd.Parameters.AddWithValue("@name", item.Name);
            Cmd.Parameters.AddWithValue("@note", item.Note);
            Cmd.Parameters.AddWithValue("@imageUrl", item.ImageUrl);
            Cmd.Parameters.AddWithValue("@categoryId", item.Category.Id);

            Con.Open();

            var result = await Cmd.ExecuteNonQueryAsync();

            Con.Close();

            if (result == 1)
                return item;

            throw new Exception("Unexpected error");
        }

        public async Task DeleteItem(int id)
        {
            Cmd.CommandText = "delete from item where id = @id";
            Cmd.Parameters.AddWithValue("@id", id);

            Con.Open();

            var result = await Cmd.ExecuteNonQueryAsync();

            Con.Close();

            if (result == 0)
                throw new Exception($"Item with id of {id} doesn't exist");
        }
    }
}
