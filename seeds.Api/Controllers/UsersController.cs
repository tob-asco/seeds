using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;

namespace seeds.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly seedsApiContext _context;
    private readonly IMapper mapper;

    public UsersController(
        seedsApiContext context,
        IMapper mapper)
    {
        _context = context;
        this.mapper = mapper;
    }

    // GET: api/Users/dummyName
    [HttpGet("{username}")]
    public async Task<ActionResult<UserFromDb>> GetUserByUsername(string username)
    {
        if (_context.User == null)
        {
            return NotFound();
        }
        var user = await _context.User
            .SingleOrDefaultAsync(u => u.Username == username);
        var userDto = mapper.Map<UserFromDb>(user);
        return userDto == null ? NotFound() : userDto;
    }

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(UserFromDb userDto)
    {
        if (_context.User == null)
        { return Problem("Entity set 'seedsApiContext.User'  is null."); }
        if (await _context.User.AnyAsync(u => u.Username == userDto.Username))
        { return Conflict("Username already exists."); }
        if (await _context.User.AnyAsync(u => u.Email == userDto.Email))
        { return Conflict("Email already exists."); }

        var user = mapper.Map<User>(userDto);

        try
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Conflict(ex);
        }
        return CreatedAtAction("GetUserByUsername", new { username = user.Username }, user);
    }
}
