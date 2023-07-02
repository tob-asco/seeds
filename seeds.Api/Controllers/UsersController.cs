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
    public async Task<ActionResult<User>> GetUserByUsername(string username)
    {
        if (_context.User == null)
        {
            return NotFound();
        }
        var user = await _context.User
            .SingleOrDefaultAsync(u => u.Username == username);
        return user == null ? NotFound() : user;
    }

    // GET: api/Users/id/5
    [HttpGet("id/{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        if (_context.User == null)
        {
            return NotFound();
        }
        var user = await _context.User.FindAsync(id);
        return user == null ? NotFound() : user;
    }

    // PUT: api/Users/id/5
    [HttpPut("id/{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
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
        if (_context.User == null)
        { return Problem("Entity set 'seedsApiContext.User'  is null."); }
        if (await _context.User.AnyAsync(u => u.Username == user.Username))
        { return Conflict("Username already exists."); }
        if (await _context.User.AnyAsync(u => u.Email == user.Email))
        { return Conflict("Email already exists."); }

        _context.User.Add(user);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            return Conflict(ex);
        }
        return CreatedAtAction("GetUser", new { id = user.Id }, user);
    }

    // DELETE: api/Users/5
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteUser(int id)
    //{
    //    if (_context.User == null)
    //    {
    //        return NotFound();
    //    }
    //    var user = await _context.User.FindAsync(id);
    //    if (user == null)
    //    {
    //        return NotFound();
    //    }

    //    _context.User.Remove(user);
    //    await _context.SaveChangesAsync();

    //    return NoContent();
    //}

    private bool UserExists(int id)
    {
        return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
