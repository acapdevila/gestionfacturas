using System.ComponentModel.DataAnnotations;

namespace GestionFacturas.Dominio;

public class EditorEstadoFactura
{
    public int IdFactura { get; set; }

    public int NumeroFactura { get; set; }

    [Display(Name = "Estado")]
    public EstadoFacturaEnum EstadoFactura { get; set; }
}