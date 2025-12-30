using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

public class OilsController : Controller
{
    private readonly AppDbContext _context;

    public OilsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Oils
    public async Task<IActionResult> Index()
    {
        var oils = await _context.Oils.ToListAsync();
        return View(oils);
    }

    // GET: Oils/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var oil = await _context.Oils.FirstOrDefaultAsync(m => m.Id == id);
        if (oil == null)
            return NotFound();

        return View(oil);
    }

    // GET: Oils/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Oils/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Article,Name,Price,Quantity,Viscosity,OilType")] Oil oil)
    {
        if (ModelState.IsValid)
        {
            oil.CreatedAt = DateTime.Now;
            _context.Add(oil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(oil);
    }

    // GET: Oils/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var oil = await _context.Oils.FindAsync(id);
        if (oil == null)
            return NotFound();

        return View(oil);
    }

    // POST: Oils/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Article,Name,Price,Quantity,Viscosity,OilType")] Oil oil)
    {
        if (id != oil.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(oil);

        var existing = await _context.Oils.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Article = oil.Article;
        existing.Name = oil.Name;
        existing.Price = oil.Price;
        existing.Quantity = oil.Quantity;
        existing.Viscosity = oil.Viscosity;
        existing.OilType = oil.OilType;

        var entry = _context.Entry(existing);
        entry.Property(e => e.Article).IsModified = true;
        entry.Property(e => e.Name).IsModified = true;
        entry.Property(e => e.Price).IsModified = true;
        entry.Property(e => e.Quantity).IsModified = true;
        entry.Property(e => e.Viscosity).IsModified = true;
        entry.Property(e => e.OilType).IsModified = true;

        try
        {
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OilExists(oil.Id))
                return NotFound();
            ModelState.AddModelError(string.Empty, "Ошибка обновления: запись изменена параллельно.");
            return View(oil);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ошибка при сохранении: " + ex.Message);
            return View(oil);
        }
    }

    // GET: Oils/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var oil = await _context.Oils.FirstOrDefaultAsync(m => m.Id == id);
        if (oil == null)
            return NotFound();

        return View(oil);
    }

    // POST: Oils/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var oil = await _context.Oils.FindAsync(id);
        if (oil != null)
        {
            _context.Oils.Remove(oil);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool OilExists(int id)
    {
        return _context.Oils.Any(e => e.Id == id);
    }
}
