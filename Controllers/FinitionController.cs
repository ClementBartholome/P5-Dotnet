using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Express_Voitures.Models;

namespace Express_Voitures.Controllers
{
    public class FinitionController : Controller
    {
        private readonly AppDbContext _context;

        public FinitionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Finition
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Finition;
            return View(await appDbContext.ToListAsync());
        }

        // GET: Finition/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finition = await _context.Finition

                .FirstOrDefaultAsync(m => m.Id == id);
            if (finition == null)
            {
                return NotFound();
            }

            return View(finition);
        }

        // GET: Finition/Create
        public IActionResult Create()
        {
            ViewData["ModeleId"] = new SelectList(_context.Modele, "Id", "Id");
            return View();
        }

        // POST: Finition/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Finition finition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(finition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Voiture");
        }

        // GET: Finition/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finition = await _context.Finition.FindAsync(id);
            if (finition == null)
            {
                return NotFound();
            }
            ViewData["ModeleId"] = new SelectList(_context.Modele, "Id", "Id", finition.ModeleId);
            return View(finition);
        }

        // POST: Finition/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,ModeleId")] Finition finition)
        {
            if (id != finition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(finition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinitionExists(finition.Id))
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
            ViewData["ModeleId"] = new SelectList(_context.Modele, "Id", "Id", finition.ModeleId);
            return View(finition);
        }

        // GET: Finition/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finition = await _context.Finition
                .FirstOrDefaultAsync(m => m.Id == id);
            if (finition == null)
            {
                return NotFound();
            }

            return View(finition);
        }

        // POST: Finition/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var finition = await _context.Finition.FindAsync(id);
            if (finition != null)
            {
                _context.Finition.Remove(finition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FinitionExists(int id)
        {
            return _context.Finition.Any(e => e.Id == id);
        }
    }
}
