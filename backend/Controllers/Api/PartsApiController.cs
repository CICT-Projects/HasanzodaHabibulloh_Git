using System.Text.Json;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers.Api;

[ApiController]
[Route("api/{resource}")]
public class PartsApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public PartsApiController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/{resource}
    [HttpGet]
    public async Task<IActionResult> GetAll(string resource)
    {
        resource = resource?.ToLowerInvariant() ?? string.Empty;
        switch (resource)
        {
            case "tires": return Ok(await _context.Tires.ToListAsync());
            case "batteries": return Ok(await _context.Batteries.ToListAsync());
            case "oils": return Ok(await _context.Oils.ToListAsync());
            case "filters": return Ok(await _context.Filters.ToListAsync());
            case "cars": return Ok(await _context.Cars.ToListAsync());
            default: return NotFound();
        }
    }

    // GET: api/{resource}/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(string resource, int id)
    {
        resource = resource?.ToLowerInvariant() ?? string.Empty;
        switch (resource)
        {
            case "tires": return Ok(await _context.Tires.FindAsync(id));
            case "batteries": return Ok(await _context.Batteries.FindAsync(id));
            case "oils": return Ok(await _context.Oils.FindAsync(id));
            case "filters": return Ok(await _context.Filters.FindAsync(id));
            case "cars": return Ok(await _context.Cars.FindAsync(id));
            default: return NotFound();
        }
    }

    // POST: api/{resource}
    [HttpPost]
    public async Task<IActionResult> Create(string resource, [FromBody] JsonElement body)
    {
        resource = resource?.ToLowerInvariant() ?? string.Empty;
        try
        {
            switch (resource)
            {
                case "tires":
                {
                    var model = JsonSerializer.Deserialize<Tire>(body.GetRawText());
                    if (model == null) return BadRequest();
                    model.CreatedAt = DateTime.Now;
                    _context.Tires.Add(model);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetById), new { resource = "tires", id = model.Id }, model);
                }
                case "batteries":
                {
                    var model = JsonSerializer.Deserialize<Battery>(body.GetRawText());
                    if (model == null) return BadRequest();
                    model.CreatedAt = DateTime.Now;
                    _context.Batteries.Add(model);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetById), new { resource = "batteries", id = model.Id }, model);
                }
                case "oils":
                {
                    var model = JsonSerializer.Deserialize<Oil>(body.GetRawText());
                    if (model == null) return BadRequest();
                    model.CreatedAt = DateTime.Now;
                    _context.Oils.Add(model);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetById), new { resource = "oils", id = model.Id }, model);
                }
                case "filters":
                {
                    var model = JsonSerializer.Deserialize<Filter>(body.GetRawText());
                    if (model == null) return BadRequest();
                    model.CreatedAt = DateTime.Now;
                    _context.Filters.Add(model);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetById), new { resource = "filters", id = model.Id }, model);
                }
                case "cars":
                {
                    var model = JsonSerializer.Deserialize<Car>(body.GetRawText());
                    if (model == null) return BadRequest();
                    _context.Cars.Add(model);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetById), new { resource = "cars", id = model.Id }, model);
                }
                default: return NotFound();
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // PUT: api/{resource}/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(string resource, int id, [FromBody] JsonElement body)
    {
        resource = resource?.ToLowerInvariant() ?? string.Empty;
        try
        {
            switch (resource)
            {
                case "tires":
                {
                    var model = JsonSerializer.Deserialize<Tire>(body.GetRawText());
                    if (model == null || model.Id != id) return BadRequest();
                    var existing = await _context.Tires.FindAsync(id);
                    if (existing == null) return NotFound();
                    _context.Entry(existing).CurrentValues.SetValues(model);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                case "batteries":
                {
                    var model = JsonSerializer.Deserialize<Battery>(body.GetRawText());
                    if (model == null || model.Id != id) return BadRequest();
                    var existing = await _context.Batteries.FindAsync(id);
                    if (existing == null) return NotFound();
                    _context.Entry(existing).CurrentValues.SetValues(model);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                case "oils":
                {
                    var model = JsonSerializer.Deserialize<Oil>(body.GetRawText());
                    if (model == null || model.Id != id) return BadRequest();
                    var existing = await _context.Oils.FindAsync(id);
                    if (existing == null) return NotFound();
                    _context.Entry(existing).CurrentValues.SetValues(model);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                case "filters":
                {
                    var model = JsonSerializer.Deserialize<Filter>(body.GetRawText());
                    if (model == null || model.Id != id) return BadRequest();
                    var existing = await _context.Filters.FindAsync(id);
                    if (existing == null) return NotFound();
                    _context.Entry(existing).CurrentValues.SetValues(model);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                case "cars":
                {
                    var model = JsonSerializer.Deserialize<Car>(body.GetRawText());
                    if (model == null || model.Id != id) return BadRequest();
                    var existing = await _context.Cars.FindAsync(id);
                    if (existing == null) return NotFound();
                    _context.Entry(existing).CurrentValues.SetValues(model);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                default: return NotFound();
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // DELETE: api/{resource}/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(string resource, int id)
    {
        resource = resource?.ToLowerInvariant() ?? string.Empty;
        switch (resource)
        {
            case "tires":
            {
                var e = await _context.Tires.FindAsync(id);
                if (e == null) return NotFound();
                _context.Tires.Remove(e);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            case "batteries":
            {
                var e = await _context.Batteries.FindAsync(id);
                if (e == null) return NotFound();
                _context.Batteries.Remove(e);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            case "oils":
            {
                var e = await _context.Oils.FindAsync(id);
                if (e == null) return NotFound();
                _context.Oils.Remove(e);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            case "filters":
            {
                var e = await _context.Filters.FindAsync(id);
                if (e == null) return NotFound();
                _context.Filters.Remove(e);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            case "cars":
            {
                var e = await _context.Cars.FindAsync(id);
                if (e == null) return NotFound();
                _context.Cars.Remove(e);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            default: return NotFound();
        }
    }
}
