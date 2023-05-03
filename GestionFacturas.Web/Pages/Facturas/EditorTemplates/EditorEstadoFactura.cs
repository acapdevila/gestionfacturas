using System.ComponentModel.DataAnnotations;
using GestionFacturas.Dominio;

namespace GestionFacturas.Web.Pages.Facturas.EditorTemplates;

public class EditorEstadoFactura
{
    public int IdFactura { get; set; }

    public string NumeroFactura { get; set; } = string.Empty;

    [Display(Name = "Estado")]
    public EstadoFacturaEnum EstadoFactura { get; set; }
}