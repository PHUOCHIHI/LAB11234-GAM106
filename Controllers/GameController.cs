using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class GameController : Controller
{
    private readonly ApplicationDbContext _context;

    public GameController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Game
    public async Task<IActionResult> Index()
    {
        var levels = await _context.GameLevels.ToListAsync();
        return View(levels);
    }

    // GET: /Game/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var level = await _context.GameLevels.FirstOrDefaultAsync(m => m.LevelId == id);
        if (level == null)
        {
            return NotFound();
        }

        return View(level);
    }

    // GET: /Game/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Game/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GameLevel level)
    {
        if (ModelState.IsValid)
        {
            _context.GameLevels.Add(level);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(level);
    }

    // GET: /Game/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var level = await _context.GameLevels.FindAsync(id);
        if (level == null)
        {
            return NotFound();
        }

        return View(level);
    }

    // POST: /Game/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, GameLevel level)
    {
        if (id != level.LevelId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(level);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.GameLevels.AnyAsync(e => e.LevelId == level.LevelId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        return View(level);
    }

    // GET: /Game/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var level = await _context.GameLevels.FirstOrDefaultAsync(m => m.LevelId == id);
        if (level == null)
        {
            return NotFound();
        }

        return View(level);
    }

    // POST: /Game/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var level = await _context.GameLevels.FindAsync(id);
        if (level != null)
        {
            _context.GameLevels.Remove(level);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
