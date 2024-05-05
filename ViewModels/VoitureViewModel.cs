using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Express_Voitures.ViewModels;

public class VoitureViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le Code VIN est obligatoire.")]
    [StringLength(17, MinimumLength = 17, ErrorMessage = "Le CodeVin doit avoir 17 caractères")]
    [DisplayName("Code VIN")]
    public string CodeVin { get; set; }

    [Required(ErrorMessage = "La marque est obligatoire.")]
    public string Marque { get; set; }

    [Required(ErrorMessage = "Le modèle est obligatoire.")]
    [DisplayName("Modèle")]
    public string Modele { get; set; }

    public List<VoitureViewModel>? Voitures { get; set; }
    public List<SelectListItem>? Marques { get; set; }
    public List<SelectListItem>? Modeles { get; set; }
    public Dictionary<int, List<SelectListItem>>? ModelesParMarque { get; set; }
    public Dictionary<int, List<SelectListItem>>? FinitionsParModele { get; set; }

    [Required(ErrorMessage = "La finition est obligatoire.")]
    public string Finition { get; set; }

    [Required(ErrorMessage = "L'année est obligatoire.")]
    [Range(1990, int.MaxValue, ErrorMessage = "Veuillez entrer une année valide.")]
    [DisplayName("Année")]
    public int? Annee { get; set; }

    [Required(ErrorMessage = "La date d'achat est obligatoire.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "Veuillez entrer une date valide.")]
    [DisplayName("Date d'achat")]
    public DateTime? DateAchat { get; set; }

    [Required(ErrorMessage = "Le prix d'achat est obligatoire.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Veuillez entrer un prix d'achat valide.")]
    [DisplayName("Prix d'achat")]
    public decimal? PrixAchat { get; set; }

    public bool Disponible { get; set; }

    [Required(ErrorMessage = "La description des réparations est obligatoire.")]
    [DisplayName("Réparations")]
    public string Reparations { get; set; }

    [Required(ErrorMessage = "Le coût des réparations est obligatoire.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Veuillez entrer un coût de réparations valide.")]
    [DisplayName("Coût répa.")]
    public decimal? CoutReparations { get; set; }

    [DisplayName("Prix vente")]
    public decimal PrixVente
    {
        get
        {
            decimal? total = PrixAchat + CoutReparations + 500;
            return total ?? 0;
        }
    }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [DisplayName("Date dispo.")]
    public DateTime? DateDisponibilite { get; set; }

    public bool Vendu { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [DisplayName("Date vente")]
    public DateTime? DateVente { get; set; }
}