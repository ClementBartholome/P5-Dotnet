namespace Express_Voitures.Models
{
    public class Voiture
    {
        public int Id { get; set; }

        // e.g : 1HGCM82633A123456
        public string CodeVin { get; set; }
        public int MarqueId { get; set; }
        public int ModeleId { get; set; }
        public int FinitionId { get; set; }
        public string Reparations { get; set; }
        public decimal? CoutReparations { get; set; }
        public int? Annee { get; set; }
        public DateTime? DateAchat { get; set; }
        public decimal? PrixAchat { get; set; }
        public bool Disponible { get; set; }

        // Propriétés de navigation
        public Marque Marque { get; set; }
        public Modele Modele { get; set; }
        public Finition Finition { get; set; }
        public VoitureVente VoitureVente { get; set; }
    }
}