using System.ComponentModel.DataAnnotations;
using GestionFacturas.Dominio;
using GestionFacturas.Web.Framework;

namespace GestionFacturas.Web.Pages.Facturas.EditorTemplates;

public class EditorLineaFactura
{
    public EditorLineaFactura(int iva)
    {
        Cantidad = "1";
        PorcentajeImpuesto = iva;
    }
    public EditorLineaFactura()
    {
    }

    public EditorLineaFactura(LineaFactura linea)
    {
        Id = linea.Id;
        IdFactura = linea.IdFactura;
        Descripcion = linea.Descripcion;
        Cantidad = linea.Cantidad.ToInputDecimal();
        PrecioUnitario = linea.PrecioUnitario;
        PorcentajeImpuesto = linea.PorcentajeImpuesto;
        
    }

    public int Id { get; set; }
    public int IdFactura { get; set; }

    [Display(Name = "Concepto")]
    public string Descripcion { get; set; } = string.Empty;

    [Display(Name = "Cantidad")] 
    public string Cantidad { get; set; } = string.Empty;

    [Display(Name = "Precio unitario")]
    public decimal PrecioUnitario { get; set; }

    [Display(Name = "% IVA")]
    public int PorcentajeImpuesto { get; set; }

    public bool EstaMarcadoParaEliminar { get; set; }
}