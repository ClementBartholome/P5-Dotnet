using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Express_Voitures.ViewModels
{
    public class AnnonceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La sélection de la voiture est obligatoire.")]
        [DisplayName("Voiture")]
        public int VoitureId { get; set; }

        [Required(ErrorMessage = "La description est obligatoire.")]
        [StringLength(400, ErrorMessage = "La description ne peut pas dépasser 400 caractères.")]
        public string Description { get; set; }

        [DisplayName("Photo")]
        public string? PhotoFilePath { get; set; }

        public IFormFile? UploadedImage { get; set; }

        public List<SelectListItem>? Voitures { get; set; }

        public string? VoitureMarque { get; set; }

        public string? VoitureModele { get; set; }

        [DisplayName("Année")]
        public int? VoitureAnnee { get; set; }

        [DisplayName("Prix")]
        public decimal? VoiturePrix { get; set; }

        public string? VoiturePrixFormatted
        {
            get { return VoiturePrix?.ToString("N0")!; }
        }
    }
}