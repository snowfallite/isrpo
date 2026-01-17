using CruiseCompany.Data;
using CruiseCompany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripAPI.DTOs;

[ApiController]
[Route("api/[controller]")]
public class RouteLinersController : ControllerBase
{
    private readonly CruiseDbContext _context;

    public RouteLinersController(CruiseDbContext context)
    {
        _context = context;
    }

    // GET: api/routeliners
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RouteLinerDto>>> GetAll()
    {
        var links = await _context.RouteLiners
            .Include(rl => rl.Route)
            .Include(rl => rl.Liner)
            .Select(rl => new RouteLinerDto
            {
                RouteId = rl.RouteId,
                RouteName = rl.Route.Name,
                LinerId = rl.LinerId,
                LinerName = rl.Liner.Name,
                Season = rl.Season,
                BasePrice = rl.BasePrice
            })
            .ToListAsync();

        return Ok(links);
    }

    // POST: api/routeliners
    [HttpPost]
    public async Task<ActionResult<RouteLinerDto>> Create(RouteLinerCreateDto dto)
    {
        var rl = new RouteLiner
        {
            RouteId = dto.RouteId,
            LinerId = dto.LinerId,
            Season = dto.Season,
            BasePrice = dto.BasePrice
        };

        _context.RouteLiners.Add(rl);
        await _context.SaveChangesAsync();

        var result = new RouteLinerDto
        {
            RouteId = rl.RouteId,
            RouteName = (await _context.CruiseRoutes.FindAsync(rl.RouteId))?.Name ?? "",
            LinerId = rl.LinerId,
            LinerName = (await _context.Liners.FindAsync(rl.LinerId))?.Name ?? "",
            Season = rl.Season,
            BasePrice = rl.BasePrice
        };

        return CreatedAtAction(nameof(GetAll), new { }, result);
    }

    // PUT: api/routeliners/{routeId}/{linerId}
    [HttpPut("{routeId}/{linerId}")]
    public async Task<IActionResult> Update(int routeId, int linerId, RouteLinerCreateDto dto)
    {
        var rl = await _context.RouteLiners
            .FirstOrDefaultAsync(x => x.RouteId == routeId && x.LinerId == linerId);

        if (rl == null) return NotFound();

        rl.Season = dto.Season;
        rl.BasePrice = dto.BasePrice;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/routeliners/{routeId}/{linerId}
    [HttpDelete("{routeId}/{linerId}")]
    public async Task<IActionResult> Delete(int routeId, int linerId)
    {
        var rl = await _context.RouteLiners
            .FirstOrDefaultAsync(x => x.RouteId == routeId && x.LinerId == linerId);

        if (rl == null) return NotFound();

        _context.RouteLiners.Remove(rl);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
