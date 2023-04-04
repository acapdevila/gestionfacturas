using GestionFacturas.Dominio.Clientes;
using System.ComponentModel.DataAnnotations;

namespace GestionFacturas.Dominio;

public class EditorFactura
{
    public EditorFactura()
    {
        Lineas = new List<EditorLineaFactura>();
    }

    public int Id { get; set; }
    public string IdUsuario { get; set; } = string.Empty;

    [Required]
    [Display(Name="Serie")]
    [StringLength(50)]
    public string SerieFactura { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Número")]
    public int NumeracionFactura { get; set; } 

    [Required]
    [StringLength(50)]
    public string FormatoNumeroFactura { get; set; } = string.Empty;

    public string NumeroFactura { get { return string.Format(FormatoNumeroFactura, SerieFactura, NumeracionFactura); } }

    [Required]
    [Display(Name = "Fecha emisión")]
    public DateTime FechaEmisionFactura { get; set; }

    [Display(Name = "Fecha vencimiento")]
    public DateTime? FechaVencimientoFactura { get; set; }
        
    [Display(Name = "Plantilla")]
    [StringLength(50)]
    public string NombreArchivoPlantillaInforme { get; set; } = string.Empty;

    [Display(Name = "Forma de pago")]
    public FormaPagoEnum FormaPago { get; set; }

    [Display(Name = "Detalles forma pago")]
    [StringLength(50)]
    public string FormaPagoDetalles { get; set; } = string.Empty;

    [Display(Name = "Número de referencia")]
    public int? IdVendedor { get; set; }

    [Display(Name = "Identificación fiscal")]
    [StringLength(50)]
    public string VendedorNumeroIdentificacionFiscal { get; set; } = string.Empty;

    [Display(Name = "Nombre o empresa")]
    [StringLength(50)]
    public string VendedorNombreOEmpresa { get; set; } = string.Empty;

    [Display(Name = "Dirección")]
    [StringLength(50)]
    public string VendedorDireccion { get; set; } = string.Empty;

    [Display(Name = "Municipio")]
    [StringLength(50)]
    public string VendedorLocalidad { get; set; } = string.Empty;

    [Display(Name = "Provincia")]
    [StringLength(50)]
    public string VendedorProvincia { get; set; } = string.Empty;

    [Display(Name = "Código postal")]
    [StringLength(10)]
    public string VendedorCodigoPostal { get; set; } = string.Empty;

    [EmailAddress]
    [Display(Name = "E-mail")]
    [StringLength(50)]
    public string VendedorEmail { get; set; } = string.Empty;

    [Display(Name = "Número de referencia")]
    public int? IdComprador { get; set; }

    [Display(Name = "Identificación fiscal")]
    [StringLength(50)]
    public string CompradorNumeroIdentificacionFiscal { get; set; } = string.Empty;

    [Display(Name = "Nombre o empresa")]
    [StringLength(128)]
    public string CompradorNombreOEmpresa { get; set; } = string.Empty;

    public string CompradorDireccion {
        get { return CompradorDireccion1 + (string.IsNullOrEmpty(CompradorDireccion2) ? string.Empty : Environment.NewLine + CompradorDireccion2);  }
    }

    [Display(Name = "Dirección 1")]
    [StringLength(64)]
    public string CompradorDireccion1 { get; set; } = string.Empty;

    [Display(Name = "Dirección 2")]
    [StringLength(64)]
    public string CompradorDireccion2 { get; set; } = string.Empty;

    [Display(Name = "Municipio")]
    [StringLength(50)]
    public string CompradorLocalidad { get; set; } = string.Empty;

    [Display(Name = "Provincia")]
    [StringLength(50)]
    public string CompradorProvincia { get; set; } = string.Empty;

    [Display(Name = "Código postal")]
    [StringLength(10)]
    public string CompradorCodigoPostal { get; set; } = string.Empty;

    [EmailAddress]
    [Display(Name = "E-mail")]
    [StringLength(50)]
    public string CompradorEmail { get; set; } = string.Empty;



    public ICollection<EditorLineaFactura> Lineas { get; set; } = new List<EditorLineaFactura>();
    
    [Display(Name = "Estado")]
    public EstadoFacturaEnum EstadoFactura { get; set; }

    [StringLength(250)]
    public string Comentarios { get; set; } = string.Empty;

    [Display(Name = "Pie")]
    [StringLength(800)]
    public string ComentariosPie { get; set; } = string.Empty;

    [Display(Name = "Nota interna")]
    [StringLength(250)]
    public string ComentarioInterno { get; set; } = string.Empty;


    public int PorcentajeIvaPorDefecto { get; set; }

    public void BorrarLineasFactura()
    {
        while (Lineas.Any())
        {
            var linea = Lineas.First();
            Lineas.Remove(linea);
        }
    }

    public void AsignarDatosCliente(Cliente cliente)
    {
        IdComprador = cliente.Id;
        CompradorCodigoPostal = cliente.CodigoPostal;
        CompradorDireccion1 = cliente.Direccion1;
        CompradorDireccion2 = cliente.Direccion2;
        CompradorEmail = cliente.Email;
        CompradorLocalidad = cliente.Localidad;
        CompradorNombreOEmpresa = cliente.NombreOEmpresa;
        CompradorNumeroIdentificacionFiscal = cliente.NumeroIdentificacionFiscal;
        CompradorProvincia = cliente.Provincia;
    }
}