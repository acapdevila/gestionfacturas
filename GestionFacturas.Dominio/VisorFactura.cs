namespace GestionFacturas.Dominio;

public class VisorFactura
{
    public VisorFactura()
    {
      
    }

    public int Id { get; set; }
    public string IdUsuario { get; set; } = string.Empty;

    public string SerieFactura { get; set; } = string.Empty;
    public int NumeracionFactura { get; set; }
    public string FormatoNumeroFactura { get; set; } = string.Empty;

    public string NumeroFactura { get { return string.Format(FormatoNumeroFactura, SerieFactura, NumeracionFactura); } }

    public DateTime FechaEmisionFactura { get; set; }
    public DateTime? FechaVencimientoFactura { get; set; }

    public int? IdVendedor { get; set; }
        
    public string VendedorNumeroIdentificacionFiscal { get; set; } = string.Empty;
    public string VendedorNombreOEmpresa { get; set; } = string.Empty;
    public string VendedorDireccion { get; set; } = string.Empty;
    public string VendedorLocalidad { get; set; } = string.Empty;
    public string VendedorProvincia { get; set; } = string.Empty;
    public string VendedorCodigoPostal { get; set; } = string.Empty;


    public int? IdComprador { get; set; }
    public string CompradorNumeroIdentificacionFiscal { get; set; } = string.Empty;
    public string CompradorNombreOEmpresa { get; set; } = string.Empty;
    public string CompradorDireccion { get; set; } = string.Empty;
    public string CompradorLocalidad { get; set; } = string.Empty;
    public string CompradorProvincia { get; set; } = string.Empty;
    public string CompradorCodigoPostal { get; set; } = string.Empty;

    public FormaPagoEnum FormaPago { get; set; }
    public string FormaPagoDetalles { get; set; } = string.Empty;

    public ICollection<LineaVisorFactura> Lineas { get; set; } = new List<LineaVisorFactura>();
    
    public EstadoFacturaEnum EstadoFactura { get; set; }
    public string Comentarios { get; set; } = string.Empty;
    public string ComentariosPie { get; set; } = string.Empty;

    public string ComentarioInterno { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public decimal BaseImponible { get; set; }

    public decimal ImporteImpuestos { get; set; }

    public decimal ImporteTotal { get; set; }
        
    public void BorrarLineasFactura()
    {
        while (Lineas.Any())
        {
            var linea = Lineas.First();
            Lineas.Remove(linea);
        }
    }
}