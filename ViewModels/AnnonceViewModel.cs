using System.ComponentModel;
using Express_Voitures.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Express_Voitures.ViewModels;

public class AnnonceViewModel
{
    public int Id { get; set; }
    [DisplayName("Voiture")] public int VoitureId { get; set; }
    public string Description { get; set; }

    [DisplayName("Photo")] public string? PhotoFilePath { get; set; }
    public IFormFile? UploadedImage { get; set; }
    public List<SelectListItem>? Voitures { get; set; }
    public string? VoitureMarque { get; set; }
    public string? VoitureModele { get; set; }

    [DisplayName("Année")] public int? VoitureAnnee { get; set; }

    [DisplayName("Prix")] public decimal? VoiturePrix { get; set; }

    public string? VoiturePrixFormatted
    {
        get { return VoiturePrix?.ToString("N0")!; }
    }
}