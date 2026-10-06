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
    public class UserController : ControllerBase
    {
        private readonly IUserGrpcClient _client;

        public UserController(IUserGrpcClient client)
        {
            _client = client;
        }

        [HttpGet("GetUserLendingRate")]
        public async Task<IActionResult> GetUserLendingRate([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var resp = await _client.GetUserLendingRateAsync(fromDate, toDate);

            var dto = resp.Users.Select(u => new UserLendingRateDto
            {
                Name = u.Name,
                BorrowCount = u.BorrowCount
            }).OrderByDescending(x => x.BorrowCount);

            return Ok(dto);
        }

        [HttpGet("GetUserReadingPace")]
        public async Task<IActionResult> GetUserReadingPace([FromQuery] int borrowerId)
        {
            var resp = await _client.GetUserReadingPaceAsync(borrowerId);

            if (resp == null || resp.Pace == null || resp.Pace.Count == 0)
                return NotFound();

            var dto = resp.Pace.Select(p => new UserReadingPaceDto
            {
                BookName = p.BookName,
                ReturnDate = p.ReturnDate != null ? p.ReturnDate.ToDateTime() : (DateTime?)null,
                ReadingPace = p.ReadingPace
            });

            return Ok(dto);
        }

        [HttpGet("GetUserBorrowingPatterns")]
        public async Task<IActionResult> GetUserBorrowingPatterns([FromQuery] int bookId)
        {
            var resp = await _client.GetUserBorrowingPatternsAsync(bookId);

            var dto = resp.Patterns.Select(p => new UserBorrowingPatternDto
            {
                BookName = p.BookName
            });

            return Ok(dto);
        }
    }
}
