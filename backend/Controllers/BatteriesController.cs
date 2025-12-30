using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

public class BatteriesController : Controller
{
    private readonly AppDbContext _context;

    public BatteriesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Batteries
    public async Task<IActionResult> Index()
    {
        var batteries = await _context.Batteries.ToListAsync();
        return View(batteries);
    }

    // GET: Batteries/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var battery = await _context.Batteries.FirstOrDefaultAsync(m => m.Id == id);
        if (battery == null)
            return NotFound();

        return View(battery);
    }

    // GET: Batteries/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Batteries/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Article,Name,Price,Quantity,VoltageV,CapacityAh")] Battery battery)
    {
        if (ModelState.IsValid)
        {
            battery.CreatedAt = DateTime.Now;
            _context.Add(battery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(battery);
    }

    // GET: Batteries/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var battery = await _context.Batteries.FindAsync(id);
        if (battery == null)
            return NotFound();

        return View(battery);
    }

    // POST: Batteries/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Article,Name,Price,Quantity,VoltageV,CapacityAh")] Battery battery)
    {
        if (id != battery.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(battery);

        var existing = await _context.Batteries.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Article = battery.Article;
        existing.Name = battery.Name;
        existing.Price = battery.Price;
        existing.Quantity = battery.Quantity;
        existing.VoltageV = battery.VoltageV;
        existing.CapacityAh = battery.CapacityAh;

        var entry = _context.Entry(existing);
        entry.Property(e => e.Article).IsModified = true;
        entry.Property(e => e.Name).IsModified = true;
        entry.Property(e => e.Price).IsModified = true;
        entry.Property(e => e.Quantity).IsModified = true;
        entry.Property(e => e.VoltageV).IsModified = true;
        entry.Property(e => e.CapacityAh).IsModified = true;

        try
        {
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BatteryExists(battery.Id))
                return NotFound();
            ModelState.AddModelError(string.Empty, "Ошибка обновления: запись изменена параллельно.");
            return View(battery);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ошибка при сохранении: " + ex.Message);
            return View(battery);
        }
    }

    // GET: Batteries/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var battery = await _context.Batteries.FirstOrDefaultAsync(m => m.Id == id);
        if (battery == null)
            return NotFound();

        return View(battery);
    }

    // POST: Batteries/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var battery = await _context.Batteries.FindAsync(id);
        if (battery != null)
        {
            _context.Batteries.Remove(battery);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool BatteryExists(int id)
    {
        return _context.Batteries.Any(e => e.Id == id);
    }
}
