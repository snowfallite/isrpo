using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CruiseCompany.Data;
using CruiseCompany.Models;
using CruiseCompany.Helpers;
using CruiseCompany.DTOs;

namespace CruiseCompany.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutesController : ControllerBase
{
    private readonly CruiseDbContext _db;
    public RoutesController(CruiseDbContext db) => _db = db;

    // GET: api/routes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RouteDto>>> GetAll()
    {
        var routes = await _db.CruiseRoutes
            .Include(r => r.RouteLiners)
            .ThenInclude(rl => rl.Liner)
            .ToListAsync();

        return Ok(routes.Select(r => r.ToDto()));
    }

    // GET: api/routes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RouteDto>> GetById(int id)
    {
        var route = await _db.CruiseRoutes
            .Include(r => r.RouteLiners)
            .ThenInclude(rl => rl.Liner)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (route == null) return NotFound();
        return Ok(route.ToDto());
    }

    // POST: api/routes
    [HttpPost]
    public async Task<ActionResult<RouteDto>> Create(RouteDto dto)
    {
        var route = new CruiseRoute { Name = dto.Name, DurationDays = dto.DurationDays };
        _db.CruiseRoutes.Add(route);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = route.Id }, route.ToDto());
    }

    // PUT: api/routes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, RouteDto dto)
    {
        var route = await _db.CruiseRoutes.FindAsync(id);
        if (route == null) return NotFound();

        route.Name = dto.Name;
        route.DurationDays = dto.DurationDays;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/routes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var route = await _db.CruiseRoutes.FindAsync(id);
        if (route == null) return NotFound();

        _db.CruiseRoutes.Remove(route);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/routes/{routeId}/liners/{linerId}
    [HttpPost("{routeId}/liners/{linerId}")]
    public async Task<IActionResult> AddLiner(int routeId, int linerId)
    {
        var rl = new RouteLiner { RouteId = routeId, LinerId = linerId };
        _db.RouteLiners.Add(rl);
        await _db.SaveChangesAsync();
        return Ok();
    }

    // DELETE: api/routes/{routeId}/liners/{linerId}
    [HttpDelete("{routeId}/liners/{linerId}")]
    public async Task<IActionResult> RemoveLiner(int routeId, int linerId)
    {
        var rl = await _db.RouteLiners.FindAsync(routeId, linerId);
        if (rl == null) return NotFound();

        _db.RouteLiners.Remove(rl);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
