using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Express_Voitures.Data;
using Express_Voitures.Models;
using Express_Voitures.Services;
using Express_Voitures.ViewModels;

namespace Express_Voitures.Controllers
{
    public class AnnonceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly VoitureService _voitureService;

        public AnnonceController(AppDbContext context, VoitureService voitureService)
        {
            _context = context;
            _voitureService = voitureService;
        }

        // GET: Annonce
        public async Task<IActionResult> Index()
        {
            var annonces = await _context.Annonce
                .Include(a => a.Voiture)
                .ThenInclude(v => v.VoitureVente).Include(annonce => annonce.Voiture)
                .ThenInclude(voiture => voiture.Marque).Include(annonce => annonce.Voiture)
                .ThenInclude(voiture => voiture.Modele)
                .ToListAsync();
            var viewModel = annonces.Select(a => new AnnonceViewModel
            {
                Id = a.Id,
                VoitureId = a.VoitureId,
                VoitureMarque = a.Voiture.Marque.Nom,
                VoitureModele = a.Voiture.Modele.Nom,
                VoitureAnnee = a.Voiture.Annee,
                VoiturePrix = a.Voiture.VoitureVente.PrixVente,
                Description = a.Description,
                PhotoFilePath = a.PhotoFilePath
            }).ToList();
            

            return View(viewModel);
        }

        // GET: Annonce/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonce
                .Include(a => a.Voiture)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (annonce == null)
            {
                return NotFound();
            }

            var viewModel = new AnnonceViewModel
            {
                Id = annonce.Id,
                VoitureId = annonce.VoitureId,
                Description = annonce.Description,
                PhotoFilePath = annonce.PhotoFilePath
            };

            return View(viewModel);
        }
        
        // GET: Annonce/Create
        public IActionResult Create()
        {
            var viewModel = new AnnonceViewModel
            {
                Voitures = _context.Voiture.Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = v.Marque.Nom + " " + v.Modele.Nom
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Annonce/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnonceViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(viewModel.UploadedImage?.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await viewModel.UploadedImage?.CopyToAsync(stream)!;
                }

                var annonce = new Annonce
                {
                    VoitureId = viewModel.VoitureId,
                    Description = viewModel.Description,
                    PhotoFilePath = "/images/" + fileName
                };

                _context.Add(annonce);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Annonce/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonce.FindAsync(id);
            if (annonce == null)
            {
                return NotFound();
            }

            var viewModel = new AnnonceViewModel
            {
                Id = annonce.Id,
                VoitureId = annonce.VoitureId,
                Description = annonce.Description,
                PhotoFilePath = annonce.PhotoFilePath
            };

            ViewData["VoitureId"] = new SelectList(_context.Voiture, "Id", "Id", viewModel.VoitureId);
            return View(viewModel);
        }

        // POST: Annonce/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AnnonceViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var annonce = new Annonce
                    {
                        Id = viewModel.Id,
                        VoitureId = viewModel.VoitureId,
                        Description = viewModel.Description,
                        PhotoFilePath = viewModel.PhotoFilePath
                    };

                    _context.Update(annonce);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnonceExists(viewModel.Id))
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
            ViewData["VoitureId"] = new SelectList(_context.Voiture, "Id", "Id", viewModel.VoitureId);
            return View(viewModel);
        }

        // GET: Annonce/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonce = await _context.Annonce
                .Include(a => a.Voiture)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (annonce == null)
            {
                return NotFound();
            }

            var viewModel = new AnnonceViewModel
            {
                Id = annonce.Id,
                VoitureId = annonce.VoitureId,
                Description = annonce.Description,
                PhotoFilePath = annonce.PhotoFilePath
            };

            return View(viewModel);
        }

        // POST: Annonce/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var annonce = await _context.Annonce.FindAsync(id);
            if (annonce != null)
            {
                // Get the full path of the image
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", annonce.PhotoFilePath.TrimStart('/'));

                // Check if the file exists and delete it
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _context.Annonce.Remove(annonce);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnonceExists(int id)
        {
            return _context.Annonce.Any(e => e.Id == id);
        }
    }
}