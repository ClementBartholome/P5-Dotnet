using Express_Voitures.Models;
using Microsoft.AspNetCore.Identity;
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
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "1",
                UserName = "admin@expressvoitures.com",
                NormalizedUserName = "ADMIN",
                Email = "admin@expressvoitures.com",
                NormalizedEmail = "admin@expressvoitures.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Password123$"),
                SecurityStamp = string.Empty
            });
            
            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(new IdentityUserClaim<string>
            {
                Id = 1,
                UserId = "1",
                ClaimType = "Admin",
                ClaimValue = "true"
            });
        }
    }
}