using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PublicLibrary.API.GrpcClients;
using PublicLibrary.API.Models;

namespace PublicLibrary.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookGrpcClient _client;

        public BookController(IBookGrpcClient client)
        {
            _client = client;
        }

        [HttpGet("GetInventoryInsights")]
        public async Task<IActionResult> GetInventoryInsights()
        {
            var resp = await _client.GetInventoryInsightsAsync();

            var dto = resp.Insights.Select(i => new InventoryInsightDto
            {
                BookName = i.BookName,
                TotalBorrowed = i.TotalBorrowed
            }).OrderByDescending(x => x.TotalBorrowed);

            return Ok(dto);
        }
    }
}
