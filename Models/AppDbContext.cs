using Microsoft.EntityFrameworkCore;

namespace Express_Voitures.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<Voiture> Voiture { get; set; }
        public DbSet<Marque> Marque { get; set; }
        public DbSet<Modele> Modele { get; set; }
        public DbSet<Finition> Finition { get; set; }
        public DbSet<VoitureVente> VoitureVente { get; set; }
        public DbSet<Annonce> Annonce { get; set; }
    }
}