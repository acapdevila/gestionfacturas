using System.ComponentModel.DataAnnotations;

namespace GestionFacturas.Dominio;

public class SelectorColumnasExcelFactura
{
    public SelectorColumnasExcelFactura()
    {
           
    }

    public string IdUsuario { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Serie")]
    public string SerieFactura { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Nº Factura")]
    [StringLength(1)]
    public string NumeroFactura { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Formato")]
    [StringLength(50)]
    public string FormatoNumeroFactura { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Fecha emisión")]
    public string FechaEmisionFactura { get; set; } = string.Empty;

    [Display(Name = "Fecha vencimiento")]
    public string FechaVencimientoFactura { get; set; } = string.Empty;

    [Display(Name = "Forma de pago")]
    public string FormaPago { get; set; } = string.Empty;

    [Display(Name = "Detalles forma pago")]
    [StringLength(50)]
    public string FormaPagoDetalles { get; set; } = string.Empty;

    [Display(Name = "Número de referencia")]
    public string IdVendedor { get; set; } = string.Empty;

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

    [Display(Name = "E-mail")]
    [StringLength(50)]
    public string VendedorEmail { get; set; } = string.Empty;

    [Display(Name = "Número de referencia")]
    public string IdComprador { get; set; } = string.Empty;

    [Display(Name = "Identificación fiscal")]
    [StringLength(50)]
    public string CompradorNumeroIdentificacionFiscal { get; set; } = string.Empty;

    [Display(Name = "Nombre o empresa")]
    [StringLength(50)]
    public string CompradorNombreOEmpresa { get; set; } = string.Empty;

    [Display(Name = "Dirección")]
    [StringLength(50)]
    public string CompradorDireccion { get; set; } = string.Empty;

    [Display(Name = "Municipio")]
    [StringLength(50)]
    public string CompradorLocalidad { get; set; } = string.Empty;

    [Display(Name = "Provincia")]
    [StringLength(50)]
    public string CompradorProvincia { get; set; } = string.Empty;

    [Display(Name = "Código postal")]
    [StringLength(10)]
    public string CompradorCodigoPostal { get; set; } = string.Empty;

    [Display(Name = "E-mail")]
    [StringLength(50)]
    public string CompradorEmail { get; set; } = string.Empty;

    [Display(Name = "Estado")]
    public string EstadoFactura { get; set; } = string.Empty;

    [StringLength(250)]
    public string Comentarios { get; set; } = string.Empty;

    [Display(Name = "Pie")]
    [StringLength(800)]
    public string ComentariosPie { get; set; } = string.Empty;

    [Display(Name = "Nota interna")]
    [StringLength(250)]
    public string ComentarioInterno { get; set; } = string.Empty;

    [Required]
    [StringLength(1)]
    [Display(Name = "Concepto")]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Cantidad")]
    public string Cantidad { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Precio unitario")]
    [StringLength(1)]
    public string PrecioUnitario { get; set; } = string.Empty;

    [Required]
    [Display(Name = "% IVA")]
    public string PorcentajeImpuesto { get; set; } = string.Empty;
}