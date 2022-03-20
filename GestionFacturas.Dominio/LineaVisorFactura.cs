namespace GestionFacturas.Dominio;

public class LineaVisorFactura
{
    public int Id { get; set; }
    public int IdFactura { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal PrecioXCantidad { get; set; }
    public int PorcentajeImpuesto { get; set; }
    public decimal Importe { get; set; }
}