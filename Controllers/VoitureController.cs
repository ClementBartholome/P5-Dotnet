using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Express_Voitures.Models;
using Express_Voitures.ViewModels;

namespace Express_Voitures.Controllers
{
    public class VoitureController : Controller
    {
        private readonly AppDbContext _context;

        public VoitureController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Voiture
        public async Task<IActionResult> Index()
        {
            var voitures = await _context.Voiture
                .Include(v => v.Marque)
                .Include(m => m.Modele)
                .Include(m => m.Finition)
                .Select(v => new VoitureViewModel
                {
                    Id = v.Id,
                    CodeVin = v.CodeVin,
                    Marque = v.Marque.Nom,
                    Modele = v.Modele.Nom,
                    Finition = v.Finition.Nom,
                    Annee = v.Annee,
                    DateAchat = v.DateAchat,
                    PrixAchat = v.PrixAchat,
                    Disponible = v.Disponible,
                    Reparations = v.Reparations,
                    CoutReparations = v.CoutReparations
                })
                .ToListAsync();
            
            ViewBag.Marques = new SelectList(_context.Marque.ToList(), "Id", "Nom");
            ViewBag.Modeles = new SelectList(_context.Modele.ToList(), "Id", "Nom");
            
            return View(voitures);
        }

        // GET: Voiture/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiture = await _context.Voiture
                .Include(v => v.Marque)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voiture == null)
            {
                return NotFound();
            }

            return View(voiture);
        }
        
        // GET: Voiture/Create
        public async Task<IActionResult> Create()
        {
            var marques = await _context.Marque
                .Include(m => m.Modeles)!
                .ThenInclude(m => m.Finitions)
                .ToListAsync();
            var modeles = marques.SelectMany(m => m.Modeles).ToList();
            var finitions = modeles.SelectMany(m => m.Finitions).ToList();

            ViewBag.Marques = new SelectList(marques, "Id", "Nom");
            
            var modelesParMarque = new Dictionary<int, List<SelectListItem>>();
            foreach (var marque in marques)
            {
                var modelesDeCetteMarque = modeles.Where(m => m.MarqueId == marque.Id)
                    .Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Nom
                    }).ToList();

                modelesParMarque[marque.Id] = modelesDeCetteMarque;
            }

            ViewBag.ModelesParMarque = modelesParMarque;

            var finitionsParModele = new Dictionary<int, List<SelectListItem>>();
            foreach (var modele in modeles)
            {
                var finitionsDeCeModele = finitions.Where(f => f.ModeleId == modele.Id)
                    .Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.Nom
                    }).ToList();

                finitionsParModele[modele.Id] = finitionsDeCeModele;
            }

            ViewBag.FinitionsParModele = finitionsParModele;

            return View();
        }

        // POST: Voiture/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VoitureViewModel voitureViewModel)
        {
            if (ModelState.IsValid)
            {
                var marque = _context.Marque.FirstOrDefault(m => m.Id.ToString() == voitureViewModel.Marque);
                var modele = _context.Modele.FirstOrDefault(m => m.Id.ToString() == voitureViewModel.Modele);
                var finition = _context.Finition.FirstOrDefault(f => f.Id.ToString() == voitureViewModel.Finition);
                
                var voiture = new Voiture
                {
                    CodeVin = voitureViewModel.CodeVin,
                    MarqueId = marque.Id,
                    ModeleId = modele.Id,
                    FinitionId = finition.Id,
                    Reparations = voitureViewModel.Reparations,
                    CoutReparations = voitureViewModel.CoutReparations,
                    Annee = voitureViewModel.Annee,
                    DateAchat = voitureViewModel.DateAchat,
                    PrixAchat = voitureViewModel.PrixAchat,
                    Disponible = voitureViewModel.Disponible
                };

                _context.Add(voiture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(voitureViewModel);
        }

        // GET: Voiture/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiture = await _context.Voiture.FindAsync(id);
            if (voiture == null)
            {
                return NotFound();
            }

            ViewData["MarqueId"] = new SelectList(_context.Marque, "Id", "Id", voiture.MarqueId);
            return View(voiture);
        }

        // POST: Voiture/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,MarqueId,Annee,DateAchat,PrixAchat,Disponible")]
            Voiture voiture)
        {
            if (id != voiture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voiture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoitureExists(voiture.Id))
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

            ViewData["MarqueId"] = new SelectList(_context.Marque, "Id", "Id", voiture.MarqueId);
            return View(voiture);
        }

        // GET: Voiture/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiture = await _context.Voiture
                .Include(v => v.Marque)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voiture == null)
            {
                return NotFound();
            }

            return View(voiture);
        }

        // POST: Voiture/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var voiture = await _context.Voiture.FindAsync(id);
            if (voiture != null)
            {
                _context.Voiture.Remove(voiture);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoitureExists(int id)
        {
            return _context.Voiture.Any(e => e.Id == id);
        }
    }
}