using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlfalahApp.Server.Context;
using AlfalahApp.Server.Models;
using AlfalahApp.Server.Utility;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace AlfalahApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost("loginuser")]
        public async Task<ActionResult<User>> LoginUser(User user)
        {

            var returnedUser = await _context.Users.Where(u => u.Username == user.Username
                                                               && u.Passwordhash == Security.Encrypt(user.Passwordhash)).FirstOrDefaultAsync();

            if (returnedUser != null)
            {
                //create a claim
                var claim = new Claim(ClaimTypes.Name, returnedUser.Username);
                //create claimidentity
                var claimIdentity = new ClaimsIdentity(new[] { claim }, "serverAuth");
                //create claimPrincipal
                var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                //sign in User
                await HttpContext.SignInAsync(claimsPrincipal);                
            }

            return await Task.FromResult(returnedUser);
        }


        [HttpGet]
        public async Task<ActionResult<String>> LogOutUser()
        {
            await HttpContext.SignOutAsync();
            return "Success";
        }


        [HttpGet("getcurrentuser")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            Models.User currentUser = new User();

            if (User.Identity.IsAuthenticated)
            {
                currentUser.Username = User.FindFirstValue(ClaimTypes.Name);
            }

            return await Task.FromResult(currentUser);

        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
