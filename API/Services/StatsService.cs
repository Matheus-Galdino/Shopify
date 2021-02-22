using API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class StatsService : BaseConnection
    {
        public async Task<List<Stat<decimal>>> GetTopItems()
        {
            Cmd.CommandText = "select sum(quantity) 'total_items_add' from list_item";

            Con.Open();

            var data = await Cmd.ExecuteReaderAsync();
            data.Read();
            var totalItemsAdded = data.GetInt32(0);
            data.Close();

            Cmd.CommandText = "select top 3 (SUM(quantity) / @totalItems * 100.0) as percentage, item.name from list_item inner join Item on idItem = item.id group by item.name order by percentage desc";
            Cmd.Parameters.AddWithValue("@totalItems", decimal.Parse(totalItemsAdded.ToString("0.00")));

            data = await Cmd.ExecuteReaderAsync();            

            var topItems = new List<Stat<decimal>>();

            while (data.Read())            
                topItems.Add(new Stat<decimal>(data.GetString(1), data.GetDecimal(0)));            

            Con.Close();

            return topItems;
        }

        public async Task<List<Stat<decimal>>> GetTopCategories() 
        {
            Cmd.CommandText = "select count(*) from list_item";

            Con.Open();

            var data = await Cmd.ExecuteReaderAsync();
            data.Read();
            var timesCategoryUsed = data.GetInt32(0);
            data.Close();

            Cmd.CommandText = "select top 3 round((count(*) / @timesCategory * 100), 0) as percentage, category.name from list_item inner join item on idItem = item.id inner join category on item.categoryId = category.id group by category.name order by percentage desc";
            Cmd.Parameters.AddWithValue("@timesCategory", decimal.Parse(timesCategoryUsed.ToString("0.00")));

            data = await Cmd.ExecuteReaderAsync();

            var topCategories = new List<Stat<decimal>>();

            while (data.Read())
                topCategories.Add(new Stat<decimal>(data.GetString(1), data.GetDecimal(0)));

            Con.Close();

            return topCategories;
        }
        
        public async Task<List<Stat<int>>> GetMonthlySummary() 
        {
            var monthlySummaries = new List<Stat<int>>();
            Cmd.CommandText = "select sum(quantity) items, CONCAT(MONTH(listDate), '/', YEAR(listDate)) as date from list_item inner join list on idList = list.id group by MONTH(listDate), YEAR(listDate) order by YEAR(listDate), MONTH(listDate)";

            Con.Open();

            var data = await Cmd.ExecuteReaderAsync();            

            while (data.Read())
                monthlySummaries.Add(new Stat<int>(data.GetString(1), data.GetInt32(0)));

            Con.Close();

            return monthlySummaries;
        }
    }
}
