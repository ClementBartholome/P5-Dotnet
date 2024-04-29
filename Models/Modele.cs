namespace Express_Voitures.Models
{
    public class Modele
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int MarqueId { get; set; }
        
        // Propriétés de navigation
        public Marque Marque { get; set; }
        public ICollection<Finition> Finitions { get; set; }
    }
}