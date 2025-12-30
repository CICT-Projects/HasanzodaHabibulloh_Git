using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

public class TiresController : Controller
{
    private readonly AppDbContext _context;

    public TiresController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Tires
    public async Task<IActionResult> Index()
    {
        var tires = await _context.Tires.ToListAsync();
        return View(tires);
    }

    // GET: Tires/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var tire = await _context.Tires.FirstOrDefaultAsync(m => m.Id == id);
        if (tire == null)
            return NotFound();

        return View(tire);
    }

    // GET: Tires/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Tires/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Article,Name,Price,Quantity,DiameterInches,ProfileDescription")] Tire tire)
    {
        if (ModelState.IsValid)
        {
            tire.CreatedAt = DateTime.Now;
            _context.Add(tire);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tire);
    }

    // GET: Tires/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var tire = await _context.Tires.FindAsync(id);
        if (tire == null)
            return NotFound();

        return View(tire);
    }

    // POST: Tires/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Article,Name,Price,Quantity,DiameterInches,ProfileDescription")] Tire tire)
    {
        if (id != tire.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(tire);

        var existing = await _context.Tires.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Article = tire.Article;
        existing.Name = tire.Name;
        existing.Price = tire.Price;
        existing.Quantity = tire.Quantity;
        existing.DiameterInches = tire.DiameterInches;
        existing.ProfileDescription = tire.ProfileDescription;

        var entry = _context.Entry(existing);
        entry.Property(e => e.Article).IsModified = true;
        entry.Property(e => e.Name).IsModified = true;
        entry.Property(e => e.Price).IsModified = true;
        entry.Property(e => e.Quantity).IsModified = true;
        entry.Property(e => e.DiameterInches).IsModified = true;
        entry.Property(e => e.ProfileDescription).IsModified = true;

        try
        {
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TireExists(tire.Id))
                return NotFound();
            ModelState.AddModelError(string.Empty, "Ошибка обновления: запись изменена параллельно.");
            return View(tire);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ошибка при сохранении: " + ex.Message);
            return View(tire);
        }
    }

    // GET: Tires/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var tire = await _context.Tires.FirstOrDefaultAsync(m => m.Id == id);
        if (tire == null)
            return NotFound();

        return View(tire);
    }

    // POST: Tires/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tire = await _context.Tires.FindAsync(id);
        if (tire != null)
        {
            _context.Tires.Remove(tire);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool TireExists(int id)
    {
        return _context.Tires.Any(e => e.Id == id);
    }
}
