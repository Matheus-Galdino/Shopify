using System;
using API.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace API.Services
{
    public class CategoryService : BaseConnection
    {       
        public async Task<List<Category>> GetCategories()
        {
            var categories = new List<Category>();

            Cmd.CommandText = "select id, name from category";

            Con.Open();

            var result = await Cmd.ExecuteReaderAsync();

            while (await result.ReadAsync())
            {
                categories.Add(new Category
                {
                    Id = result.GetInt32(0),
                    Name = result.GetString(1),
                });
            }

            Con.Close();

            return categories;
        }

        public async Task<bool> CheckIfCategoryExists(int id)
        {            
            Cmd.CommandText = "select id from category where id = @id";
            Cmd.Parameters.AddWithValue("@id", id);            

            Con.Open();

            var result = await Cmd.ExecuteReaderAsync();

            var exists = result.HasRows;

            Con.Close();

            return exists;
        }

        public async Task InsertCategory(string name)
        {

            Cmd.CommandText = "insert into category(name) values(@name)";
            Cmd.Parameters.AddWithValue("@name", name);

            Con.Open();

            var result = await Cmd.ExecuteNonQueryAsync();

            Con.Close();

            if (result == 0)
                throw new Exception("Unxepected error");

        }     
    }
}
