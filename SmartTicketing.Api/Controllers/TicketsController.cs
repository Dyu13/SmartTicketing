using AutoMapper;
using AutoMapper.AspNet.OData;
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
public class TicketsController : Controller
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public TicketsController(
        IApplicationDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    // TODO: have a proper response object to map the OData response to
    // [EnableQuery(PageSize = 100)] // Suggested by automapper to not use it because it will duplicate data
    [HttpGet]
    public async Task<IActionResult> Get(ODataQueryOptions<Ticket> options) // using ODataQueryOptions to apply the select/filter straight on db through queryable using ef core
    {
        var ticketQuery = await _dbContext.Tickets.GetQueryAsync(_mapper, options);
        var tickets = ticketQuery.Take(100); // since PageSize is not applied anymore, I force 100 on IQueryable so that it will be applied on the database after the skip/filtering
        return Ok(tickets);
    }

    [HttpGet("~/api/ticketsCount")]
    public async Task<ActionResult<int>> Count()
    {
        var ticketsCount = _dbContext.Tickets.Count();
        return Ok(ticketsCount);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Post([FromBody] Ticket ticket)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _dbContext.Tickets.AddAsync(ticket);
        await _dbContext.SaveChangesAsync();

        return Ok(ticket.TicketId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] Ticket ticket)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (id != ticket.TicketId)
            return BadRequest("Id do not match");

        _dbContext.Tickets.Update(ticket);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var ticket = await _dbContext.Tickets.FirstOrDefaultAsync(t => t.TicketId == id);

        if (ticket == null)
            return BadRequest($"No ticket found with id {id}");

        _dbContext.Tickets.Remove(ticket);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}
