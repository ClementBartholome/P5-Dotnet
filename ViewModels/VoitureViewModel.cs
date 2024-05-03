using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Express_Voitures.ViewModels;

public class VoitureViewModel
{
    public int Id { get; set; }

    [StringLength(17, MinimumLength = 17, ErrorMessage = "Le CodeVin doit avoir 17 caractères")]
    [DisplayName("Code VIN")]
    public string CodeVin { get; set; }

    public string Marque { get; set; }
    
    [DisplayName("Modèle")]
    public string Modele { get; set; }
    public List<VoitureViewModel>? Voitures { get; set; }
    public List<SelectListItem>? Marques { get; set; }
    public List<SelectListItem>? Modeles { get; set; }
    public Dictionary<int, List<SelectListItem>>? ModelesParMarque { get; set; }
    public Dictionary<int, List<SelectListItem>>? FinitionsParModele { get; set; }
    public string Finition { get; set; }

    [Range(1990, int.MaxValue, ErrorMessage = "L'année ne peut pas être antérieure à 1990")]
    [DisplayName("Année")]
    public int Annee { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [DisplayName("Date d'achat")]
    public DateTime DateAchat { get; set; }

    [DisplayName("Prix d'achat")] public decimal PrixAchat { get; set; }
    public bool Disponible { get; set; }
    [DisplayName("Réparations")] public string Reparations { get; set; }
    [DisplayName("Coût répa.")] public decimal CoutReparations { get; set; }

    [DisplayName("Prix vente")]
    public string PrixVente
    {
        get
        {
            decimal total = PrixAchat + CoutReparations + 500;
            return total.ToString("N", CultureInfo.GetCultureInfo("fr-FR"));
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