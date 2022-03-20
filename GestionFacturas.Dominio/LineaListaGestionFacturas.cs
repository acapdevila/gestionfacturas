namespace GestionFacturas.Dominio;

public class LineaListaGestionFacturas
{
    public int Id { get; set; }
    public string IdUsuario { get; set; } = string.Empty;

    public string SerieFactura { get; set; } = string.Empty;
    public int NumeracionFactura { get; set; }
    public string FormatoNumeroFactura { get; set; } = string.Empty;

    public string NumeroFactura { get { return string.Format(FormatoNumeroFactura, SerieFactura, NumeracionFactura); } }

    public DateTime FechaEmisionFactura { get; set; }
    public DateTime? FechaVencimientoFactura { get; set; }

    public IEnumerable<string> ListaDescripciones { get; set; } = new List<string>();

    public string Conceptos { get {
        return string.Join(", ", ListaDescripciones);
    } }

    public int? IdComprador { get; set; }
    public string CompradorNombreOEmpresa { get; set; } = string.Empty;

    public string CompradorNombreComercial { get; set; } = string.Empty;

    public string CompradorNombre { get { return CompradorNombreComercial ?? CompradorNombreOEmpresa; } }

    public decimal BaseImponible { get; set; }
    public decimal Impuestos { get; set; }
    public decimal ImporteTotal {
        get { return BaseImponible + Impuestos; }
    }

    public EstadoFacturaEnum EstadoFactura { get; set; }

}