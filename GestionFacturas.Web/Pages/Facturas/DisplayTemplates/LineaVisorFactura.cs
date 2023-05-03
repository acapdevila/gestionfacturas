using GestionFacturas.Dominio;

namespace GestionFacturas.Web.Pages.Facturas.DisplayTemplates;

public class LineaVisorFactura
{
    public LineaVisorFactura()
    {
        
    }

    public LineaVisorFactura(LineaFactura linea)
    {
        Descripcion = linea.Descripcion;
        Cantidad = linea.Cantidad;
        PrecioUnitario = linea.PrecioUnitario;
        PorcentajeImpuesto = linea.PorcentajeImpuesto;
        Importe = linea.Importe;
        
    }
    
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int PorcentajeImpuesto { get; set; }
    public decimal Importe { get; set; }
}