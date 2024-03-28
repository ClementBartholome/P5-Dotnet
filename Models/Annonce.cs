namespace Express_Voitures.Models
{
    public class Annonce
    {
        public int Id { get; set; }
        public int VoitureId { get; set; }
        public Voiture Voiture { get; set; }
        public string Description { get; set; }
        public string PhotoFilePath { get; set; }
    }
}