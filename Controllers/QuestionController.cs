using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class QuestionController : Controller
{
    private readonly ApplicationDbContext _context;

    public QuestionController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Question
    public async Task<IActionResult> Index()
    {
        var questions = await _context.Questions.ToListAsync();
        return View(questions);
    }

    // GET: /Question/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var question = await _context.Questions.FirstOrDefaultAsync(q => q.QuestionId == id);
        if (question == null)
        {
            return NotFound();
        }

        return View(question);
    }

    // GET: /Question/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Question/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Question question)
    {
        if (ModelState.IsValid)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(question);
    }

    // GET: /Question/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var question = await _context.Questions.FindAsync(id);
        if (question == null)
        {
            return NotFound();
        }

        return View(question);
    }

    // POST: /Question/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Question question)
    {
        if (id != question.QuestionId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(question);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Questions.AnyAsync(q => q.QuestionId == question.QuestionId))
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

        return View(question);
    }

    // GET: /Question/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var question = await _context.Questions.FirstOrDefaultAsync(q => q.QuestionId == id);
        if (question == null)
        {
            return NotFound();
        }

        return View(question);
    }

    // POST: /Question/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var question = await _context.Questions.FindAsync(id);
        if (question != null)
        {
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
