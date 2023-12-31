﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;
using System.Web;

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
    public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
    {
        // decode the string
        username = HttpUtility.UrlDecode(username);

        // search for the user
        var user = await _context.User
            .SingleOrDefaultAsync(u => u.Username == username);
        if(user == null)
        {
            // username doesn't exist
            Response.Headers.Add("X-Error-Type", "DbRecordNotFound");
            return NotFound();
        }

        // convert to DTO
        var userDto = mapper.Map<UserDto>(user);
        return userDto;
    }

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(UserDto userDto)
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
