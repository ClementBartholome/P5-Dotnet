namespace Express_Voitures.Models
{
    public class Marque
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public ICollection<Modele> Modeles { get; set; }
        public ICollection<Voiture> Voitures { get; set; }
    }
}