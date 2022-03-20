namespace GestionFacturas.Dominio;

public class LineaFactura
{
    public int Id { get; set; }
    public int IdFactura { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int PorcentajeImpuesto { get; set; }

    public decimal PrecioXCantidad { get { return PrecioUnitario * Cantidad; } }
    public decimal ImporteImpuesto { get { return Math.Round(((PrecioXCantidad * PorcentajeImpuesto) /100), 2); } }
    public decimal Importe { get { return PrecioXCantidad + ImporteImpuesto;  } }

    public virtual Factura Factura { get; set; } = new();
}