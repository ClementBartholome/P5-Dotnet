using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Express_Voitures.Services;
using Express_Voitures.ViewModels;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;

namespace Express_Voitures.Controllers
{
    public class AnnonceController : Controller
    {
        private readonly VoitureService _voitureService;
        private readonly AnnonceService _annonceService;
        private readonly IConfiguration _configuration;


        public AnnonceController(VoitureService voitureService, AnnonceService annonceService, IConfiguration configuration)
        {
            _voitureService = voitureService;
            _annonceService = annonceService;
            _configuration = configuration;
        }

        // GET: Annonce
        public async Task<IActionResult> Index(int? page)
        {
            var annonces = await _annonceService.GetAllAnnonces();
            var viewModel = annonces.Select(a => new AnnonceViewModel
            {
                Id = a.Id,
                VoitureId = a.VoitureId,
                VoitureMarque = a.Voiture?.Marque?.Nom,
                VoitureModele = a.Voiture?.Modele?.Nom,
                VoitureAnnee = a.Voiture?.Annee,
                VoiturePrix = a.Voiture?.VoitureVente?.PrixVente,
                Description = a.Description,
                PhotoFilePath = a.PhotoFilePath
            }).ToList();

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            return View(viewModel.ToPagedList(pageNumber, pageSize));
        }

        // GET: Annonce/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonce = await _annonceService.GetAnnonceById(id);

            if (annonce != null)
            {
                var viewModel = new AnnonceViewModel
                {
                    Id = annonce.Id,
                    VoitureId = annonce.VoitureId,
                    Description = annonce.Description,
                    PhotoFilePath = annonce.PhotoFilePath
                };

                return View(viewModel);
            }

            return NotFound();
        }

        // GET: Annonce/Create
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            var viewModel = new AnnonceViewModel
            {
                Voitures = _voitureService.GetVoituresDisponibles()
            };

            return View(viewModel);
        }

        // POST: Annonce/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create(AnnonceViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var existingAnnonce = await _annonceService.GetAnnonceByVoitureId(viewModel.VoitureId);
                if (existingAnnonce != null)
                {
                    ModelState.AddModelError("", "Une annonce existe déjà pour cette voiture.");
                    viewModel.Voitures = _voitureService.GetVoituresDisponibles();
                    return View(viewModel);
                }

                var fileName = Path.GetFileName(viewModel.UploadedImage?.FileName);

                var blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("AzureBlobStorage"));

                var containerClient = blobServiceClient.GetBlobContainerClient("images");

                var blobClient = containerClient.GetBlobClient(fileName);

                using (var stream = viewModel.UploadedImage.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                var blobUri = blobClient.Uri.AbsoluteUri;

                await _annonceService.CreateAnnonce(viewModel, blobUri);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Annonce/Edit/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonce = await _annonceService.GetAnnonceById(id);
            if (annonce == null)
            {
                return NotFound();
            }

            var viewModel = new AnnonceViewModel
            {
                Id = annonce.Id,
                VoitureId = annonce.VoitureId,
                Description = annonce.Description,
                PhotoFilePath = annonce.PhotoFilePath,
                Voitures = _voitureService.GetVoituresForSelectList()
            };

            return View(viewModel);
        }

        // POST: Annonce/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int id, AnnonceViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(viewModel);
            try
            {
                if (viewModel.UploadedImage != null)
                {
                    var fileName = Path.GetFileName(viewModel.UploadedImage.FileName);

                    var blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("AzureBlobStorage"));

                    var containerClient = blobServiceClient.GetBlobContainerClient("images");

                    var blobClient = containerClient.GetBlobClient(fileName);

                    using (var stream = viewModel.UploadedImage.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, overwrite: true);
                    }

                    var blobUri = blobClient.Uri.AbsoluteUri;

                    await _annonceService.UpdateAnnonce(viewModel, blobUri);
                }
                else
                {
                    await _annonceService.UpdateAnnonce(viewModel, null);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnonceExists(viewModel.Id))
                {
                    return NotFound();
                }

                throw new Exception("Erreur de mise à jour de l'annonce");
            }
        }


        // GET: Annonce/Delete/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonce = await _annonceService.GetAnnonceById(id);
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
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var annonce = await _annonceService.GetAnnonceById(id);
            if (annonce == null) return RedirectToAction(nameof(Index));
            // Get the full path of the image
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                annonce.PhotoFilePath.TrimStart('/'));

            // Check if the file exists and delete it
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            await _annonceService.DeleteAnnonce(annonce);


            return RedirectToAction(nameof(Index));
        }

        private bool AnnonceExists(int id)
        {
            return _annonceService.GetAnnonceById(id) != null;
        }
    }
}