using Express_Voitures.Data;
using Express_Voitures.Models;

namespace Express_Voitures.Services;

public class ModeleService
{
    private readonly AppDbContext _context;
    
    public ModeleService(AppDbContext context)
    {
        _context = context;
    }
    
    public List<Modele> GetModelesByMarqueId(int marqueId)
    {
        var modeles = _context.Modele.Where(m => m.MarqueId == marqueId).ToList();

        if (modeles == null)
        {
            throw new Exception("Modèles introuvables");
        }
        
        return modeles;
    }
    
    public Modele GetModeleById(int id)
    {
        return _context.Modele.FirstOrDefault(m => m.Id == id);
    }
}