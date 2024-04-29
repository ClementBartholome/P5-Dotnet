using Express_Voitures.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Express_Voitures.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
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