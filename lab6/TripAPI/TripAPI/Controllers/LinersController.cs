using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CruiseCompany.Data;
using CruiseCompany.Models;
using CruiseCompany.Helpers;
using CruiseCompany.DTOs;

namespace CruiseCompany.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LinersController : ControllerBase
{
    private readonly CruiseDbContext _db;
    public LinersController(CruiseDbContext db) => _db = db;

    // GET: api/liners
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LinerDto>>> GetAll()
    {
        var liners = await _db.Liners
            .Include(l => l.RouteLiners)
            .ThenInclude(rl => rl.Route)
            .ToListAsync();

        return Ok(liners.Select(l => l.ToDto()));
    }

    // GET: api/liners/5
    [HttpGet("{id}")]
    public async Task<ActionResult<LinerDto>> GetById(int id)
    {
        var liner = await _db.Liners
            .Include(l => l.RouteLiners)
            .ThenInclude(rl => rl.Route)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (liner == null) return NotFound();
        return Ok(liner.ToDto());
    }

    // POST: api/liners
    [HttpPost]
    public async Task<ActionResult<LinerDto>> Create(LinerDto dto)
    {
        var liner = new Liner
        {
            Name = dto.Name,
            Capacity = dto.Capacity,
            Class = dto.Class,
            YearBuilt = dto.YearBuilt
        };
        _db.Liners.Add(liner);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = liner.Id }, liner.ToDto());
    }

    // PUT: api/liners/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, LinerDto dto)
    {
        var liner = await _db.Liners.FindAsync(id);
        if (liner == null) return NotFound();

        liner.Name = dto.Name;
        liner.Capacity = dto.Capacity;
        liner.Class = dto.Class;
        liner.YearBuilt = dto.YearBuilt;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/liners/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var liner = await _db.Liners.FindAsync(id);
        if (liner == null) return NotFound();

        _db.Liners.Remove(liner);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
