namespace WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.LegacyUsers
            .Include(u => u.role)
            .Include(u => u.region)
            .ToListAsync();
    }

    // GET: api/User/all
    // Trả về cả danh sách LegacyUsers (user cũ) và Identity ApplicationUsers (AspNetUsers)
    [HttpGet("all")]
    public async Task<ActionResult<object>> GetAllUsers()
    {
        var legacyUsers = await _context.LegacyUsers
            .Include(u => u.role)
            .Include(u => u.region)
            .ToListAsync();

        var identityUsers = await _context.Users.ToListAsync();

        return Ok(new
        {
            legacyUsers,
            identityUsers
        });
    }

    // GET: api/User/identity
    // Trả về đầy đủ toàn bộ user Identity (bảng AspNetUsers)
    [HttpGet("identity")]
    public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetIdentityUsers()
    {
        var identityUsers = await _context.Users.ToListAsync();
        return Ok(identityUsers);
    }

    // GET: api/User/identity/{id}
    [HttpGet("identity/{id}")]
    public async Task<ActionResult<ApplicationUser>> GetIdentityUser(string id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    //Post: Login api/User/login
    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _context.LegacyUsers
            .Include(u => u.role)
            .Include(u => u.region)
            .FirstOrDefaultAsync(u => u.username == loginRequest.Username && u.password == loginRequest.Password);  
        if (user == null)
        {
            return Unauthorized();
        }
        return user;
    }

    // PUT: api/User/identity/{id}
    [HttpPut("identity/{id}")]
    public async Task<IActionResult> UpdateIdentityUser(string id, [FromBody] ApplicationUser updatedUser)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        // Update allowed fields
        user.Name = updatedUser.Name;
        user.RegionId = updatedUser.RegionId;
        user.Avatar = updatedUser.Avatar;
        user.IsDeleted = updatedUser.IsDeleted;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.LegacyUsers.Include(u => u.role).Include(u => u.region).FirstOrDefaultAsync(u => u.userId == id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // POST: api/User
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        // Add new user to the database
        _context.LegacyUsers.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { id = user.userId}, user);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.userId)
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
            return NotFound();
        }

        return NoContent();
    }
}