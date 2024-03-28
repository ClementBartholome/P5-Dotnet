namespace Express_Voitures.Models
{
    public class Voiture
    {
        public int Id { get; set; }
        public int MarqueId { get; set; }
        public Marque Marque { get; set; }
        public int Annee { get; set; }
        public DateTime DateAchat { get; set; }
        public decimal PrixAchat { get; set; }
        public bool Disponible { get; set; }
    }
}