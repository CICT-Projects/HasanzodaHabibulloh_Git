using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

public class FiltersController : Controller
{
    private readonly AppDbContext _context;

    public FiltersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Filters
    public async Task<IActionResult> Index()
    {
        var filters = await _context.Filters.ToListAsync();
        return View(filters);
    }

    // GET: Filters/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var filter = await _context.Filters.FirstOrDefaultAsync(m => m.Id == id);
        if (filter == null)
            return NotFound();

        return View(filter);
    }

    // GET: Filters/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Filters/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Article,Name,Price,Quantity,FilterType,ApprovedSize")] Filter filter)
    {
        if (ModelState.IsValid)
        {
            filter.CreatedAt = DateTime.Now;
            _context.Add(filter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(filter);
    }

    // GET: Filters/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var filter = await _context.Filters.FindAsync(id);
        if (filter == null)
            return NotFound();

        return View(filter);
    }

    // POST: Filters/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Article,Name,Price,Quantity,FilterType,ApprovedSize")] Filter filter)
    {
        if (id != filter.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(filter);

        var existing = await _context.Filters.FindAsync(id);
        if (existing == null)
            return NotFound();

        // Обновляем только изменяемые поля, не трогаем CreatedAt
        existing.Article = filter.Article;
        existing.Name = filter.Name;
        existing.Price = filter.Price;
        existing.Quantity = filter.Quantity;
        existing.FilterType = filter.FilterType;
        existing.ApprovedSize = filter.ApprovedSize;

        // Явно помечаем свойства как изменённые, чтобы EF точно их сохранил
        var entry = _context.Entry(existing);
        entry.Property(e => e.Article).IsModified = true;
        entry.Property(e => e.Name).IsModified = true;
        entry.Property(e => e.Price).IsModified = true;
        entry.Property(e => e.Quantity).IsModified = true;
        entry.Property(e => e.FilterType).IsModified = true;
        entry.Property(e => e.ApprovedSize).IsModified = true;

        try
        {
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FilterExists(filter.Id))
                return NotFound();
            ModelState.AddModelError(string.Empty, "Ошибка обновления: запись была изменена параллельно.");
            return View(filter);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ошибка при сохранении: " + ex.Message);
            return View(filter);
        }
    }

    // GET: Filters/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var filter = await _context.Filters.FirstOrDefaultAsync(m => m.Id == id);
        if (filter == null)
            return NotFound();

        return View(filter);
    }

    // POST: Filters/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var filter = await _context.Filters.FindAsync(id);
        if (filter != null)
        {
            _context.Filters.Remove(filter);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool FilterExists(int id)
    {
        return _context.Filters.Any(e => e.Id == id);
    }
}
