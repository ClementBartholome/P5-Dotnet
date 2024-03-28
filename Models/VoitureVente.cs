namespace Express_Voitures.Models
{
    public class VoitureVente
    {
        public int Id { get; set; }
        public int VoitureId { get; set; }
        public Voiture Voiture { get; set; }
        public DateTime DateDisponibilite { get; set; }
        public DateTime DateVente { get; set; }
        public decimal PrixVente { get; set; }
    }
}