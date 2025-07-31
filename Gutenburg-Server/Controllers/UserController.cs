using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gutenburg_Server.Data;
using Gutenburg_Server.Models;

namespace Gutenburg_Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    public UserController(AppDbContext context)
    {
        _context = context;
    }

    //Get 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUser()
    {
        return await _context.Users.ToListAsync();
    }
    //Get ID
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user is null ? NotFound() : Ok(user);
    }
    // POST
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> EditUser(int id, User user)
    {
        if (id == user.UserId) return BadRequest();
        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Users.Any(user => user.UserId == id)) return NotFound(); throw;
        }
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return NotFound();
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}