using System.ComponentModel.DataAnnotations;

namespace GestionFacturas.Dominio;

public class EditorEstadoFactura
{
    public int IdFactura { get; set; }

    public string NumeroFactura { get; set; } = string.Empty;

    [Display(Name = "Estado")]
    public EstadoFacturaEnum EstadoFactura { get; set; }
}