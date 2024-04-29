using Express_Voitures.Data;
using Express_Voitures.Models;
using Microsoft.EntityFrameworkCore;

namespace Express_Voitures.Services;

public class MarqueService
{
    private readonly AppDbContext _context;

    public MarqueService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Marque>> GetAllMarques()
    {
        return await _context.Marque
            .Include(m => m.Modeles)!
            .ThenInclude(m => m.Finitions)
            .ToListAsync();
    }
    
    public Marque GetMarqueById(int id)
    {
        return _context.Marque.FirstOrDefault(m => m.Id == id);
    }
}