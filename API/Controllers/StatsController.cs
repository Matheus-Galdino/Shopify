using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("stats")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly StatsService _statsService = new StatsService();

        [HttpGet("items")]
        public async Task<List<Stat<decimal>>> GetTopItems() => await _statsService.GetTopItems();

        [HttpGet("categories")]
        public async Task<List<Stat<decimal>>> GetTopCategories() => await _statsService.GetTopCategories();

        [HttpGet("monthly")]
        public async Task<List<Stat<int>>> GetMonthlySummary() => await _statsService.GetMonthlySummary();
    }
}
