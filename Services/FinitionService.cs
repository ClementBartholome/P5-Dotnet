﻿using Express_Voitures.Data;
using Express_Voitures.Models;

namespace Express_Voitures.Services;

public class FinitionService
{
    private readonly AppDbContext _context;

    public FinitionService(AppDbContext context)
    {
        _context = context;
    }

    public List<Finition> GetFinitionsByModeleId(int modeleId)
    {
        var finitions = _context.Finition.Where(f => f.ModeleId == modeleId).ToList();

        if (finitions == null)
        {
            throw new Exception("Finitions introuvables");
        }

        return finitions;
    }
    
    public Finition GetFinitionById(int id)
    {
        return _context.Finition.FirstOrDefault(f => f.Id == id);
    }
}