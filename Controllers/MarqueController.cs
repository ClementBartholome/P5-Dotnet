using Express_Voitures.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Express_Voitures.Models;
using Express_Voitures.Services;
using Microsoft.AspNetCore.Authorization;

namespace Express_Voitures.Controllers
{
    public class MarqueController : Controller
    {
        private readonly AppDbContext _context;
        private readonly MarqueService _marqueService;
        
        public MarqueController(AppDbContext context, MarqueService marqueService)
        {
            _context = context;
            _marqueService = marqueService;
        }

        // GET: Marque
        public async Task<IActionResult> Index()
        {
            return View(await _context.Marque.ToListAsync());
        }

        // GET: Marque/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marque = await _context.Marque
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marque == null)
            {
                return NotFound();
            }

            return View(marque);
        }

        // GET: Marque/Create
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Marque/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Nom")] Marque marque)
        {
            if (ModelState.IsValid)
            {
                if (_marqueService.MarqueExists(marque.Nom))
                {
                    TempData["ErrorMessage"] = "Une marque avec ce nom existe déjà.";
                    return RedirectToAction("Index", "Voiture");
                }   
                _context.Add(marque);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Voiture");
            }
            return View(marque);
        }

        // GET: Marque/Edit/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marque = await _context.Marque.FindAsync(id);
            if (marque == null)
            {
                return NotFound();
            }
            return View(marque);
        }

        // POST: Marque/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] Marque marque)
        {
            if (id != marque.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marque);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarqueExists(marque.Id))
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
            return View(marque);
        }

        // GET: Marque/Delete/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marque = await _context.Marque
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marque == null)
            {
                return NotFound();
            }

            return View(marque);
        }

        // POST: Marque/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var marque = await _context.Marque.FindAsync(id);
            if (marque != null)
            {
                _context.Marque.Remove(marque);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarqueExists(int id)
        {
            return _context.Marque.Any(e => e.Id == id);
        }
    }
}
