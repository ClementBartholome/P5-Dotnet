using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Express_Voitures.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Express_Voitures.Models;
using Express_Voitures.Services;
using Express_Voitures.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Express_Voitures.Controllers
{
    public class ModeleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ModeleService _modeleService;

        public ModeleController(AppDbContext context, ModeleService modeleService)
        {
            _context = context;
            _modeleService = modeleService;
        }

        // GET: Modele
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Modele.Include(m => m.Marque);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Modele/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modele = await _context.Modele
                .Include(m => m.Marque)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modele == null)
            {
                return NotFound();
            }

            return View(modele);
        }

        // GET: Modele/Create
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Modele/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create(ModeleViewModel modeleViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_modeleService.ModeleExists(modeleViewModel.Nom, modeleViewModel.MarqueId))
                {
                    TempData["ErrorMessage"] = "Un modèle avec ce nom existe déjà pour cette marque.";
                    return RedirectToAction("Index", "Voiture");
                }
                
                var marque = _context.Marque.FirstOrDefault(m => m.Id == modeleViewModel.MarqueId);
                var modele = new Modele
                {
                    Nom = modeleViewModel.Nom,
                    MarqueId = marque.Id
                };
                
                _context.Add(modele);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Voiture");
            }

            return RedirectToAction("Index", "Voiture");
        }

        // GET: Modele/Edit/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modele = await _context.Modele.FindAsync(id);
            if (modele == null)
            {
                return NotFound();
            }
            ViewData["MarqueId"] = new SelectList(_context.Marque, "Id", "Id", modele.MarqueId);
            return View(modele);
        }

        // POST: Modele/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,MarqueId")] Modele modele)
        {
            if (id != modele.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modele);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModeleExists(modele.Id))
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
            ViewData["MarqueId"] = new SelectList(_context.Marque, "Id", "Id", modele.MarqueId);
            return View(modele);
        }

        // GET: Modele/Delete/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modele = await _context.Modele
                .Include(m => m.Marque)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modele == null)
            {
                return NotFound();
            }

            return View(modele);
        }

        // POST: Modele/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modele = await _context.Modele.FindAsync(id);
            if (modele != null)
            {
                _context.Modele.Remove(modele);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModeleExists(int id)
        {
            return _context.Modele.Any(e => e.Id == id);
        }
    }
}
