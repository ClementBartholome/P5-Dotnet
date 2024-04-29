using Express_Voitures.Data;
using Express_Voitures.Models;
using Express_Voitures.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Express_Voitures.Services;

public class VoitureService
{
    private readonly AppDbContext _context;

    public VoitureService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<VoitureViewModel>> GetAllVoitures()
    {
        return await _context.Voiture
            .Include(v => v.Marque)
            .Include(m => m.Modele)
            .Include(m => m.Finition)
            .Include(v => v.VoitureVente)
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
                CoutReparations = v.CoutReparations,
                DateDisponibilite = v.VoitureVente != null ? v.VoitureVente.DateDisponibilite : null,
                Vendu = v.VoitureVente != null ? v.VoitureVente.Vendu : false,
                DateVente = v.VoitureVente != null ? v.VoitureVente.DateVente : null
            })
            .ToListAsync();
    }

    public async Task<Voiture> GetVoitureById(int id)
    {
        var voiture = await _context.Voiture
            .Include(v => v.Marque)
            .Include(m => m.Modele)
            .Include(m => m.Finition)
            .Include(v => v.VoitureVente)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (voiture == null)
        {
            throw new Exception("Voiture introuvable");
        }

        return voiture;
    }
    
    public List<SelectListItem> GetVoituresForSelectList()
    {
        return _context.Voiture.Select(v => new SelectListItem
        {
            Value = v.Id.ToString(),
            Text = v.Marque.Nom + " " + v.Modele.Nom
        }).ToList();
    }

    public List<SelectListItem> GetVoituresDisponibles()
    {
        return _context.Voiture
            .Where(v => v.Disponible && !_context.Annonce.Any(a => a.VoitureId == v.Id))
            .Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Marque.Nom + " " + v.Modele.Nom + " " + v.CodeVin
            }).ToList();
    }

    public void CreateOrUpdateVoiture(VoitureViewModel voitureViewModel, Voiture? voitureToUpdate = null)
    {
        var voiture = voitureToUpdate ?? new Voiture();

        voiture.CodeVin = voitureViewModel.CodeVin;
        voiture.MarqueId = int.Parse(voitureViewModel.Marque);
        voiture.ModeleId = int.Parse(voitureViewModel.Modele);
        voiture.FinitionId = int.Parse(voitureViewModel.Finition);
        voiture.Reparations = voitureViewModel.Reparations;
        voiture.CoutReparations = voitureViewModel.CoutReparations;
        voiture.Annee = voitureViewModel.Annee;
        voiture.DateAchat = voitureViewModel.DateAchat;
        voiture.PrixAchat = voitureViewModel.PrixAchat;
        voiture.Disponible = voitureViewModel.Disponible;
        
        if (voitureToUpdate == null)
        {
            _context.Voiture.Add(voiture);
        }
        else
        {
            _context.Voiture.Update(voiture);
        }

        if (voitureViewModel.Disponible)
        {
            var voitureVente = _context.VoitureVente.FirstOrDefault(vv => vv.VoitureId == voiture.Id);

            if (voitureVente == null)
            {
                voitureVente = new VoitureVente
                {
                    Voiture = voiture,
                    DateDisponibilite = DateTime.Now,
                    Vendu = voitureViewModel.Vendu,
                    PrixVente = Convert.ToDecimal(voitureViewModel.PrixVente)
                };

                _context.VoitureVente.Add(voitureVente);
            }
            else
            {
                voitureVente.DateDisponibilite = (DateTime)voitureViewModel.DateDisponibilite!;
                voitureVente.Vendu = voitureViewModel.Vendu;
                voitureVente.DateVente = voitureViewModel.DateVente;
                voitureVente.PrixVente = Convert.ToDecimal(voitureViewModel.PrixVente);

                _context.VoitureVente.Update(voitureVente);
            }
        }
        
        if (voitureViewModel.Vendu)
        {
            var voitureVente = _context.VoitureVente.FirstOrDefault(vv => vv.VoitureId == voiture.Id);
            
            if (voitureVente == null)
            {
                voitureVente = new VoitureVente
                {
                    Voiture = voiture,
                    DateDisponibilite = DateTime.Now,
                    Vendu = true,
                    DateVente = voitureViewModel.DateVente,
                    PrixVente = Convert.ToDecimal(voitureViewModel.PrixVente)
                };

                _context.VoitureVente.Add(voitureVente);
            }
            else
            {
                voitureVente.DateVente = voitureViewModel.DateVente;
                voitureVente.PrixVente = Convert.ToDecimal(voitureViewModel.PrixVente);
                voitureVente.Vendu = true;

                _context.VoitureVente.Update(voitureVente);
            }
            
            var annonce = _context.Annonce.FirstOrDefault(a => a.VoitureId == voiture.Id);
            if (annonce != null)
            {
                _context.Annonce.Remove(annonce);
            }
        }

        _context.SaveChanges();
    }
    
    public async Task<bool> IsVinUnique(string vin, int? voitureId = null)
    {
        if (voitureId == null)
        {
            return !await _context.Voiture.AnyAsync(v => v.CodeVin == vin);
        }

        return !await _context.Voiture.AnyAsync(v => v.CodeVin == vin && v.Id != voitureId);
    }
}