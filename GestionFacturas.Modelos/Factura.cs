using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Modelos
{
    public class Factura
    {
        public Factura()
        {
            Lineas = new List<LineaFactura>();
        }
        
        public int Id { get; set; }
        public string IdUsuario { get; set; }
        
        public string SerieFactura { get; set; }
        public int NumeracionFactura { get; set; }
        public string FormatoNumeroFactura { get; set; }

        public string NumeroFactura { get { return string.Format(FormatoNumeroFactura ?? "", SerieFactura ?? "", NumeracionFactura); } }
               
        public DateTime FechaEmisionFactura { get; set; }
        public DateTime? FechaVencimientoFactura { get; set; }

        [Display(Name = "Forma de pago")]
        public FormaPagoEnum FormaPago { get; set; }

        [Display(Name = "Detalles forma pago")]
        public string FormaPagoDetalles { get; set; }

        public int? IdVendedor { get; set; }
        public string VendedorNumeroIdentificacionFiscal { get; set; }
        public string VendedorNombreOEmpresa { get; set; }
        public string VendedorDireccion { get; set; }
        public string VendedorLocalidad { get; set; }
        public string VendedorProvincia { get; set; }
        public string VendedorCodigoPostal { get; set; }
        public string VendedorEmail { get; set; }

        public int? IdComprador { get; set; }
        public string CompradorNumeroIdentificacionFiscal { get; set; }
        public string CompradorNombreOEmpresa { get; set; }
        public string CompradorDireccion { get; set; }

        private string[] LineasCompradorDireccion => CompradorDireccion.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

        public string CompradorDireccion1 => LineasCompradorDireccion.FirstOrDefault();

        public string CompradorDireccion2 => LineasCompradorDireccion.Length > 1 ? LineasCompradorDireccion[1] : string.Empty;

        public string CompradorLocalidad { get; set; }
        public string CompradorProvincia { get; set; }
        public string CompradorCodigoPostal { get; set; }

        public string CompradorEmail { get; set; }

        public virtual ICollection<LineaFactura> Lineas { get; set; }
        public virtual Usuario Usuario { get; set; }
        
        public EstadoFacturaEnum EstadoFactura { get; set; }
        public string Comentarios { get; set; }
        public string ComentariosPie { get; set; }
        public string ComentarioInterno { get; set; }
        
        public string NombreArchivoPlantillaInforme { get; set; }

        public string Titulo
        {
            get
            {
                return string.Format("Factura {0} {1}", NumeroFactura, CompradorNombreOEmpresa);
            }
        }

        public decimal BaseImponible
        {
            get
            {
                if (!Lineas.Any()) return 0;

                return Lineas.Sum(m => m.PrecioXCantidad);
            }
        }

        public decimal ImporteImpuestos
        {
            get
            {
                if (!Lineas.Any()) return 0;

                return Lineas.Sum(m => m.ImporteImpuesto);
            }
        }

        public decimal ImporteTotal
        {
            get
            {
                if (!Lineas.Any()) return 0;

                return BaseImponible + ImporteImpuestos;
            }
        }

        public virtual Cliente Comprador { get; set; }
    }

    public enum EstadoFacturaEnum
    {
        Borrador = 1,
        Enviada = 2,
        Cobrada = 3,
        Transferida = 4
    }

    public enum FormaPagoEnum
    {
        [Display(Name = @"Sin definir")]
        SinDefinir = 0,
        Transferencia = 1,
        Tarjeta = 2,
        Efectivo = 3,
        [Display(Name = @"Domiciliación")]
        Domiciliacion = 4
    }

    public class LineaFactura
    {
        public int Id { get; set; }
        public int IdFactura { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int PorcentajeImpuesto { get; set; }

        public decimal PrecioXCantidad { get { return PrecioUnitario * Cantidad; } }
        public decimal ImporteImpuesto { get { return Math.Round(((PrecioXCantidad * PorcentajeImpuesto) /100), 2); } }
        public decimal Importe { get { return PrecioXCantidad + ImporteImpuesto;  } }

        public virtual Factura Factura { get; set; }
    }

    public class LineaListaGestionFacturas
    {
        public int Id { get; set; }
        public string IdUsuario { get; set; }

        public string SerieFactura { get; set; }
        public int NumeracionFactura { get; set; }
        public string FormatoNumeroFactura { get; set; }

        public string NumeroFactura { get { return string.Format(FormatoNumeroFactura, SerieFactura, NumeracionFactura); } }

        public DateTime FechaEmisionFactura { get; set; }
        public DateTime? FechaVencimientoFactura { get; set; }

        public IEnumerable<string> ListaDescripciones { get; set; }

        public string Conceptos { get {
                return string.Join(", ", ListaDescripciones);
            } }

        public int? IdComprador { get; set; }
        public string CompradorNombreOEmpresa { get; set; }

        public string CompradorNombreComercial { get; set; }

        public string CompradorNombre { get { return CompradorNombreComercial ?? CompradorNombreOEmpresa; } }

        public decimal BaseImponible { get; set; }
        public decimal Impuestos { get; set; }
        public decimal ImporteTotal {
            get { return BaseImponible + Impuestos; }
        }

        public EstadoFacturaEnum EstadoFactura { get; set; }

    }

    public class EditorFactura
    {
        public EditorFactura()
        {
            Lineas = new List<EditorLineaFactura>();
        }

        public int Id { get; set; }
        public string IdUsuario { get; set; }

        [Required]
        [Display(Name="Serie")]
        [StringLength(50)]
        public string SerieFactura { get; set; }

        [Required]
        [Display(Name = "Número")]
        public int NumeracionFactura { get; set; }

        [Required]
        [StringLength(50)]
        public string FormatoNumeroFactura { get; set; }

        public string NumeroFactura { get { return string.Format(FormatoNumeroFactura, SerieFactura, NumeracionFactura); } }

        [Required]
        [Display(Name = "Fecha emisión")]
        public DateTime FechaEmisionFactura { get; set; }

        [Display(Name = "Fecha vencimiento")]
        public DateTime? FechaVencimientoFactura { get; set; }
        
        [Display(Name = "Plantilla")]
        [StringLength(50)]
        public string NombreArchivoPlantillaInforme { get; set; }
        
        [Display(Name = "Forma de pago")]
        public FormaPagoEnum FormaPago { get; set; }

        [Display(Name = "Detalles forma pago")]
        [StringLength(50)]
        public string FormaPagoDetalles { get; set; }

        [Display(Name = "Número de referencia")]
        public int? IdVendedor { get; set; }

        [Display(Name = "Identificación fiscal")]
        [StringLength(50)]
        public string VendedorNumeroIdentificacionFiscal { get; set; }

        [Display(Name = "Nombre o empresa")]
        [StringLength(50)]
        public string VendedorNombreOEmpresa { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(50)]
        public string VendedorDireccion { get; set; }

        [Display(Name = "Municipio")]
        [StringLength(50)]
        public string VendedorLocalidad { get; set; }

        [Display(Name = "Provincia")]
        [StringLength(50)]
        public string VendedorProvincia { get; set; }

        [Display(Name = "Código postal")]
        [StringLength(10)]
        public string VendedorCodigoPostal { get; set; }

        [EmailAddress]
        [Display(Name = "E-mail")]
        [StringLength(50)]
        public string VendedorEmail { get; set; }

        [Display(Name = "Número de referencia")]
        public int? IdComprador { get; set; }

        [Display(Name = "Identificación fiscal")]
        [StringLength(50)]
        public string CompradorNumeroIdentificacionFiscal { get; set; }

        [Display(Name = "Nombre o empresa")]
        [StringLength(50)]
        public string CompradorNombreOEmpresa { get; set; }

        public string CompradorDireccion {
            get { return CompradorDireccion1 + (string.IsNullOrEmpty(CompradorDireccion2) ? string.Empty : Environment.NewLine + CompradorDireccion2);  }
        }

        [Display(Name = "Dirección 1")]
        [StringLength(64)]
        public string CompradorDireccion1 { get; set; }

        [Display(Name = "Dirección 2")]
        [StringLength(64)]
        public string CompradorDireccion2 { get; set; }

        [Display(Name = "Municipio")]
        [StringLength(50)]
        public string CompradorLocalidad { get; set; }

        [Display(Name = "Provincia")]
        [StringLength(50)]
        public string CompradorProvincia { get; set; }

        [Display(Name = "Código postal")]
        [StringLength(10)]
        public string CompradorCodigoPostal { get; set; }

        [EmailAddress]
        [Display(Name = "E-mail")]
        [StringLength(50)]
        public string CompradorEmail { get; set; }



        public ICollection<EditorLineaFactura> Lineas { get; set; }
        public Usuario Usuario { get; set; }

        [Display(Name = "Estado")]
        public EstadoFacturaEnum EstadoFactura { get; set; }

        [StringLength(250)]
        public string Comentarios { get; set; }

        [Display(Name = "Pie")]
        [StringLength(800)]
        public string ComentariosPie { get; set; }

        [Display(Name = "Nota interna")]
        [StringLength(250)]
        public string ComentarioInterno { get; set; }


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


    public class EditorEstadoFactura
    {
        public int IdFactura { get; set; }

        [Display(Name = "Estado")]
        public EstadoFacturaEnum EstadoFactura { get; set; }
    }

    public class VisorFactura
    {
        public VisorFactura()
        {
            Lineas = new List<LineaVisorFactura>();
        }

        public int Id { get; set; }
        public string IdUsuario { get; set; }

        public string SerieFactura { get; set; }
        public int NumeracionFactura { get; set; }
        public string FormatoNumeroFactura { get; set; }

        public string NumeroFactura { get { return string.Format(FormatoNumeroFactura, SerieFactura, NumeracionFactura); } }

        public DateTime FechaEmisionFactura { get; set; }
        public DateTime? FechaVencimientoFactura { get; set; }

        public int? IdVendedor { get; set; }
        
        public string VendedorNumeroIdentificacionFiscal { get; set; }
        public string VendedorNombreOEmpresa { get; set; }
        public string VendedorDireccion { get; set; }
        public string VendedorLocalidad { get; set; }
        public string VendedorProvincia { get; set; }
        public string VendedorCodigoPostal { get; set; }

     
        public int? IdComprador { get; set; }
        public string CompradorNumeroIdentificacionFiscal { get; set; }
        public string CompradorNombreOEmpresa { get; set; }
        public string CompradorDireccion { get; set; }
        public string CompradorLocalidad { get; set; }
        public string CompradorProvincia { get; set; }
        public string CompradorCodigoPostal { get; set; }

        public FormaPagoEnum FormaPago { get; set; }
        public string FormaPagoDetalles { get; set; }

        public ICollection<LineaVisorFactura> Lineas { get; set; }
        public Usuario Usuario { get; set; }

        public EstadoFacturaEnum EstadoFactura { get; set; }
        public string Comentarios { get; set; }
        public string ComentariosPie { get; set; }

        public string ComentarioInterno { get; set; }

        public string Titulo { get; set; }

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

    public class EditorLineaFactura
    {
        public EditorLineaFactura(int iva)
        {
            Cantidad = 1;
            PorcentajeImpuesto = iva;
        }
        public EditorLineaFactura()
        {
        }

        
        public int Id { get; set; }
        public int IdFactura { get; set; }

        [Display(Name = "Concepto")]
        public string Descripcion { get; set; }

        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Display(Name = "Precio unitario")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "% IVA")]
        public int PorcentajeImpuesto { get; set; }

        public bool EstaMarcadoParaEliminar { get; set; }
    }

    public class LineaVisorFactura
    {
        public int Id { get; set; }
        public int IdFactura { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioXCantidad { get; set; }
        public int PorcentajeImpuesto { get; set; }
        public decimal Importe { get; set; }
    }

    public class SelectorColumnasExcelFactura
    {
        public SelectorColumnasExcelFactura()
        {
           
        }

        public string IdUsuario { get; set; }

        [Required]
        [Display(Name = "Serie")]
        public string SerieFactura { get; set; }

        [Required]
        [Display(Name = "Nº Factura")]
        [StringLength(1)]
        public string NumeroFactura { get; set; }

        [Required]
        [Display(Name = "Formato")]
        [StringLength(50)]
        public string FormatoNumeroFactura { get; set; }

        [Required]
        [Display(Name = "Fecha emisión")]
        public string FechaEmisionFactura { get; set; }

        [Display(Name = "Fecha vencimiento")]
        public string FechaVencimientoFactura { get; set; }

        [Display(Name = "Forma de pago")]
        public string FormaPago { get; set; }

        [Display(Name = "Detalles forma pago")]
        [StringLength(50)]
        public string FormaPagoDetalles { get; set; }

        [Display(Name = "Número de referencia")]
        public string IdVendedor { get; set; }

        [Display(Name = "Identificación fiscal")]
        [StringLength(50)]
        public string VendedorNumeroIdentificacionFiscal { get; set; }

        [Display(Name = "Nombre o empresa")]
        [StringLength(50)]
        public string VendedorNombreOEmpresa { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(50)]
        public string VendedorDireccion { get; set; }

        [Display(Name = "Municipio")]
        [StringLength(50)]
        public string VendedorLocalidad { get; set; }

        [Display(Name = "Provincia")]
        [StringLength(50)]
        public string VendedorProvincia { get; set; }

        [Display(Name = "Código postal")]
        [StringLength(10)]
        public string VendedorCodigoPostal { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(50)]
        public string VendedorEmail { get; set; }

        [Display(Name = "Número de referencia")]
        public string IdComprador { get; set; }

        [Display(Name = "Identificación fiscal")]
        [StringLength(50)]
        public string CompradorNumeroIdentificacionFiscal { get; set; }

        [Display(Name = "Nombre o empresa")]
        [StringLength(50)]
        public string CompradorNombreOEmpresa { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(50)]
        public string CompradorDireccion { get; set; }

        [Display(Name = "Municipio")]
        [StringLength(50)]
        public string CompradorLocalidad { get; set; }

        [Display(Name = "Provincia")]
        [StringLength(50)]
        public string CompradorProvincia { get; set; }

        [Display(Name = "Código postal")]
        [StringLength(10)]
        public string CompradorCodigoPostal { get; set; }
        
        [Display(Name = "E-mail")]
        [StringLength(50)]
        public string CompradorEmail { get; set; }
        
        [Display(Name = "Estado")]
        public string EstadoFactura { get; set; }

        [StringLength(250)]
        public string Comentarios { get; set; }

        [Display(Name = "Pie")]
        [StringLength(800)]
        public string ComentariosPie { get; set; }

        [Display(Name = "Nota interna")]
        [StringLength(250)]
        public string ComentarioInterno { get; set; }

        [Required]
        [StringLength(1)]
        [Display(Name = "Concepto")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public string Cantidad { get; set; }

        [Required]
        [Display(Name = "Precio unitario")]
        [StringLength(1)]
        public string PrecioUnitario { get; set; }

        [Required]
        [Display(Name = "% IVA")]
        public string PorcentajeImpuesto { get; set; }
    }

    public class TotalesFacturas
    {        
        public decimal TotalBaseImponible { get; set; }
        public decimal TotalImpuestos { get; set; }
        public decimal TotalImporte {
            get { return TotalBaseImponible + TotalImpuestos;  }
        }
    }

}
