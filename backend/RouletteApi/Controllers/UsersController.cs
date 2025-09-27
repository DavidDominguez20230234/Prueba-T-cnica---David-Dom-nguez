using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouletteApi.Data;
using RouletteApi.Models;

namespace RouletteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
       private readonly AppDbContext _appDbContext;

        public UsersController(AppDbContext appDbContext) => _appDbContext = appDbContext;

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name) 
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            if (user == null) return NotFound();
            return Ok( new 
            {
                user.Name,
                user.Balance
            });
        }

        [HttpGet("saved-games")]
        public async Task<IEnumerable<User>> GetSavedGames() 
        {
            var users = await _appDbContext.Users.OrderByDescending(x => x.Id).ToListAsync();

            return users;

        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SavePayload payload) 
        {
            var normalizedName = payload.Name.Trim().ToLower();

            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Name.ToLower() == normalizedName);

            if (user == null)
            {
                user = new User
                {
                    Name = normalizedName,
                    Balance = payload.Balance
                };

                _appDbContext.Users.Add(user);
            }
            else 
            {
                user.Balance = payload.Balance;
            }

            await _appDbContext.SaveChangesAsync();
            return Ok(new 
            {
                user.Name,
                user.Balance
            });
        }
    }
}
