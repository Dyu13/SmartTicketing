using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

using SmartTicketing.Application.Interfaces;
using SmartTicketing.Domain;

namespace SmartTicketing.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly IApplicationDbContext _dbContext;

    // TODO: only allow Admin users to perform CRUD operations

    public UsersController(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<List<User>>> Get()
    {
        return Ok(_dbContext.Users);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Post([FromBody] User user)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return Ok(user.UserId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] User user)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (id != user.UserId)
            return BadRequest("Id do not match");

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);

        if (user == null)
            return BadRequest($"No user found with id {id}");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}
