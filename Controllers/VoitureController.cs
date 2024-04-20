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
    public class VoitureController : Controller
    {
        private readonly AppDbContext _context;
        private readonly VoitureService _voitureService;
        private readonly MarqueService _marqueService;
        private readonly ModeleService _modeleService;
        private readonly FinitionService _finitionService;
        private readonly AnnonceService _annonceService;


        public VoitureController(AppDbContext context, VoitureService voitureService, MarqueService marqueService,
            ModeleService modeleService, FinitionService finitionService, AnnonceService annonceService)
        {
            _context = context;
            _voitureService = voitureService;
            _marqueService = marqueService;
            _modeleService = modeleService;
            _finitionService = finitionService;
            _annonceService = annonceService;
        }

        // GET: Voiture
        public async Task<IActionResult> Index()
        {
            var voitures = await _voitureService.GetAllVoitures();
            var marques = new SelectList(_context.Marque.ToList(), "Id", "Nom");
            var modeles = new SelectList(_context.Modele.ToList(), "Id", "Nom");

            var viewModel = new VoitureViewModel
            {
                Voitures = voitures,
                Marques = marques.ToList(),
                Modeles = modeles.ToList()
            };

            return View(viewModel);
        }

        // GET: Voiture/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var voiture = await _voitureService.GetVoitureById(id);
            return View(voiture);
        }

        // GET: Voiture/Create
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create()
        {
            await FillViewBag();
            return View();
        }

        // POST: Voiture/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create(VoitureViewModel voitureViewModel)
        {
            if (ModelState.IsValid)
            {
                var isVinUnique = await _voitureService.IsVinUnique(voitureViewModel.CodeVin);
                    
                if (!isVinUnique)
                {
                    ModelState.AddModelError("CodeVin", "Ce Code VIN est déjà utilisé");
                    return View(voitureViewModel);
                }
                
                _voitureService.CreateOrUpdateVoiture(voitureViewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(voitureViewModel);
        }

        // GET: Voiture/Edit/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var voiture = await _voitureService.GetVoitureById(id);

            var voitureViewModel = new VoitureViewModel
            {
                Id = voiture.Id,
                CodeVin = voiture.CodeVin,
                Marque = voiture.Marque.Nom,
                Modele = voiture.Modele.Nom,
                Finition = voiture.Finition.Nom,
                Reparations = voiture.Reparations,
                CoutReparations = voiture.CoutReparations,
                Annee = voiture.Annee,
                DateAchat = voiture.DateAchat,
                PrixAchat = voiture.PrixAchat,
                Disponible = voiture.Disponible,
                DateDisponibilite = voiture.VoitureVente?.DateDisponibilite,
                Vendu = voiture.VoitureVente?.Vendu ?? false,
                DateVente = voiture.VoitureVente?.DateVente,
            };

            if (voitureViewModel.Disponible)
            {
                var voitureVente = new VoitureVente
                {
                    Voiture = voiture,
                    DateDisponibilite = DateTime.Now,
                    Vendu = voitureViewModel.Vendu,
                };

                if (voitureViewModel.Vendu)
                {
                    voitureVente.DateVente = voitureViewModel.DateVente;
                    voitureVente.PrixVente = decimal.Parse(voitureViewModel.PrixVente);
                    voitureVente.Vendu = voitureViewModel.Vendu;
                }
            }

            await FillViewBag();

            return View(voitureViewModel);
        }

        // POST: Voiture/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int id, VoitureViewModel voitureViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (voitureViewModel.Vendu)
                    {
                        var annonce = await _annonceService.GetAnnonceByVoitureId(voitureViewModel.Id);
                        if (annonce != null)
                        {
                            await _annonceService.DeleteAnnonce(annonce);
                        }
                    }
                    
                    var voiture = await _voitureService.GetVoitureById(id);
                    _voitureService.CreateOrUpdateVoiture(voitureViewModel, voiture);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoitureExists(voitureViewModel.Id))
                    {
                        return NotFound();
                    }

                    throw new Exception("Erreur de mise à jour de la voiture");
                }

                return RedirectToAction(nameof(Index));
            }

            return View(voitureViewModel);
        }

        // GET: Voiture/Delete/5
        [Authorize(Policy = "Admin")]
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
        [Authorize(Policy = "Admin")]
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

        private async Task FillViewBag()
        {
            var marques = await _marqueService.GetAllMarques();
            ViewBag.Marques = new SelectList(marques, "Id", "Nom");

            var modelesParMarque = new Dictionary<int, List<SelectListItem>>();
            foreach (var marque in marques)
            {
                var modelesDeCetteMarque = _modeleService.GetModelesByMarqueId(marque.Id)
                    .Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Nom
                    }).ToList();

                modelesParMarque[marque.Id] = modelesDeCetteMarque;
            }

            ViewBag.ModelesParMarque = modelesParMarque;

            var finitionsParModele = new Dictionary<int, List<SelectListItem>>();
            foreach (var modele in _context.Modele)
            {
                var finitionsDeCeModele = _finitionService.GetFinitionsByModeleId(modele.Id)
                    .Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.Nom
                    }).ToList();

                finitionsParModele[modele.Id] = finitionsDeCeModele;
            }

            ViewBag.FinitionsParModele = finitionsParModele;
        }
    }
}