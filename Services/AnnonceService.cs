using Express_Voitures.Data;
using Express_Voitures.Models;
using Express_Voitures.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Express_Voitures.Services;

public class AnnonceService
{
    private readonly AppDbContext _context;

    public AnnonceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Annonce>> GetAllAnnonces()
    {
        var annonces = await _context.Annonce
            .Include(a => a.Voiture)
            .ThenInclude(v => v.VoitureVente)
            .Include(annonce => annonce.Voiture)
            .ThenInclude(voiture => voiture.Marque)
            .Include(annonce => annonce.Voiture)
            .ThenInclude(voiture => voiture.Modele)
            .OrderBy(a => a.Id)
            .ToListAsync();
        return annonces;
    }

    public async Task CreateAnnonce(AnnonceViewModel viewModel, string filePath)
    {
        var annonce = new Annonce
        {
            VoitureId = viewModel.VoitureId,
            Description = viewModel.Description,
            PhotoFilePath = filePath
        };


        _context.Annonce.Add(annonce);


        await _context.SaveChangesAsync();
    }


    public async Task<Annonce?> GetAnnonceById(int? id)
    {
        var annonce = await _context.Annonce
            .Include(a => a.Voiture)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (annonce == null)
        {
            throw new Exception("Annonce introuvable");
        }

        return annonce;
    }

    public async Task DeleteAnnonce(Annonce annonce)
    {
        _context.Remove(annonce);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAnnonce(AnnonceViewModel viewModel, string fileName)
    {
        var annonce = await GetAnnonceById(viewModel.Id);

        if (annonce != null)
        {
            annonce.VoitureId = viewModel.VoitureId;
            annonce.Description = viewModel.Description;

            if (viewModel.UploadedImage != null)
            {
                annonce.PhotoFilePath = fileName;
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task<Annonce?> GetAnnonceByVoitureId(int voitureId)
    {
        var annonce = await _context.Annonce.FirstOrDefaultAsync(a => a.VoitureId == voitureId);
        
        return annonce;
    }
}