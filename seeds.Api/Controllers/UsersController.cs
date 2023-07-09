using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Model;

namespace seeds.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly seedsApiContext _context;

    public UsersController(seedsApiContext context)
    {
        _context = context;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return _context.User == null ? 
            NotFound() : await _context.User.ToListAsync();
    }

    // GET: api/Users/dummyName
    [HttpGet("{username}")]
    public async Task<ActionResult<User>> GetUserByUsernameAsync(string username)
    {
        if (_context.User == null)
        {
            return NotFound();
        }
        var user = await _context.User
            .SingleOrDefaultAsync(u => u.Username == username);
        return user == null ? NotFound() : user;
    }

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<User>> PostUserAsync(User user)
    {
        if (_context.User == null)
        { return Problem("Entity set 'seedsApiContext.User'  is null."); }
        if (await _context.User.AnyAsync(u => u.Username == user.Username))
        { return Conflict("Username already exists."); }
        if (await _context.User.AnyAsync(u => u.Email == user.Email))
        { return Conflict("Email already exists."); }

        try
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            return Conflict(ex);
        }
        return CreatedAtAction("GetUser", new { username = user.Username }, user);
    }
}
