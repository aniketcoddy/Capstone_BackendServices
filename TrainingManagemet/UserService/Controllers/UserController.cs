using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersContext _context;

        public UsersController(UsersContext context)
        {
            _context = context;
        }

        // GET: api/users?status=true
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Where(user => user.Status == true)
                .ToListAsync();
        }

        // GET: api/users/search?query={searchQuery}
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<User>>> SearchUsers(string query)
        {
            var users = await _context.Users
                .Where(u => u.Status == true &&
                            (u.Name.Contains(query) || u.Email.Contains(query) || u.Role.ToString() == query))
                .ToListAsync();

            return users;
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Status == false)
            {
                return BadRequest("User is not active.");
            }

            return user;
        }
    }
}
