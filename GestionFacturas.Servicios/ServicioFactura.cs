using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using Microsoft.Reporting.WebForms;

namespace GestionFacturas.Servicios
{
    public class ServicioFactura : ServicioCrudFactura
    {
        private ServicioEmail _servicioEmail;
        
        private int PorcentajeIvaPorDefecto
        {
            get { return 21; }
        }

        public ServicioFactura(ContextoBaseDatos contexto, ServicioEmail servicioEmail) : base(contexto)
        {
            _servicioEmail = servicioEmail;
        }

        public async Task<IEnumerable<LineaListaGestionFacturas>> ListaGestionFacturasAsync(FiltroBusquedaFactura filtroBusqueda)
        {
            var consulta = _contexto.Facturas.AsQueryable();

            if (filtroBusqueda.TieneValores)
            {
                if (!string.IsNullOrEmpty(filtroBusqueda.NombreOEmpresaCliente))
                {
                    consulta = consulta.Where(m => m.CompradorNombreOEmpresa.Contains(filtroBusqueda.NombreOEmpresaCliente));
                }

                if (filtroBusqueda.FechaDesde.HasValue && filtroBusqueda.FechaHasta.HasValue)
                {
                    consulta = consulta.Where(m => m.FechaEmisionFactura >= filtroBusqueda.FechaDesde.Value && m.FechaEmisionFactura <= filtroBusqueda.FechaHasta.Value);
                }

                if (!string.IsNullOrEmpty(filtroBusqueda.NombreArchivoLogo))
                {
                    consulta = consulta.Where(m => m.NombreArchivoLogo.Contains(filtroBusqueda.NombreArchivoLogo));
                }
            }

            var consultaFacturas = consulta
                .Select(m => new LineaListaGestionFacturas
                {
                    Id = m.Id,
                    IdUsuario = m.IdUsuario,
                    IdComprador = m.IdComprador,
                    FormatoNumeroFactura = m.FormatoNumeroFactura,
                    NumeracionFactura = m.NumeracionFactura,
                    SerieFactura = m.SerieFactura,
                    FechaEmisionFactura = m.FechaEmisionFactura,
                    FechaVencimientoFactura = m.FechaVencimientoFactura,
                    EstadoFactura = m.EstadoFactura,
                    BaseImponible = m.Lineas.Sum(l => (decimal?)(l.PrecioUnitario * l.Cantidad)) ?? 0,
                    Impuestos = m.Lineas.Sum(l => (decimal?)(l.PrecioUnitario * l.Cantidad * l.PorcentajeImpuesto / 100)) ?? 0,
                    ImporteTotal = m.Lineas.Sum(l => (decimal?)((l.PrecioUnitario * l.Cantidad) + (l.PrecioUnitario * l.Cantidad * l.PorcentajeImpuesto / 100))) ?? 0,
                    CompradorNombreOEmpresa = m.CompradorNombreOEmpresa,
                    ListaDescripciones = m.Lineas.Select(l=>l.Descripcion)
                });

            var facturas = await consultaFacturas.ToListAsync();

            return facturas;
        }

        public async Task<EditorFactura> ObtenerEditorFacturaParaCrearNuevaFactura(string serie)
        {
            var ultimaFacturaCreada = await ObtenerUlitmaFacturaDeLaSerie(serie);

            if (ultimaFacturaCreada == null)
            {
                return new EditorFactura
                {
                    SerieFactura = serie,
                    NumeracionFactura = 1,
                    FormatoNumeroFactura = "{0}{1:1000}",
                    FechaEmisionFactura = DateTime.Today,
                    PorcentajeIvaPorDefecto = PorcentajeIvaPorDefecto,
                    FormaPago = FormaPagoEnum.Transferencia,

                    Lineas = new List<EditorLineaFactura> {
                            new EditorLineaFactura {
                                    Cantidad = 1,
                                    PorcentajeImpuesto = PorcentajeIvaPorDefecto
                            }
                      }
                };
             }

            return new EditorFactura
            {
                SerieFactura = ultimaFacturaCreada.SerieFactura,
                NumeracionFactura = ultimaFacturaCreada.NumeracionFactura + 1,
                FormatoNumeroFactura = ultimaFacturaCreada.FormatoNumeroFactura,
                FechaEmisionFactura = DateTime.Today,
                NombreArchivoLogo = ultimaFacturaCreada.NombreArchivoLogo,
                PorcentajeIvaPorDefecto = PorcentajeIvaPorDefecto,
                FormaPago = ultimaFacturaCreada.FormaPago,
                FormaPagoDetalles = ultimaFacturaCreada.FormaPagoDetalles,
                ComentariosPie = ultimaFacturaCreada.ComentariosPie,
                EstadoFactura = EstadoFacturaEnum.Creada,
                IdVendedor = ultimaFacturaCreada.IdVendedor,
                VendedorCodigoPostal = ultimaFacturaCreada.VendedorCodigoPostal,
                VendedorDireccion = ultimaFacturaCreada.VendedorDireccion,
                VendedorEmail = ultimaFacturaCreada.VendedorEmail,
                VendedorLocalidad = ultimaFacturaCreada.VendedorLocalidad,
                VendedorNombreOEmpresa = ultimaFacturaCreada.VendedorNombreOEmpresa,
                VendedorNumeroIdentificacionFiscal = ultimaFacturaCreada.VendedorNumeroIdentificacionFiscal,
                VendedorProvincia = ultimaFacturaCreada.VendedorProvincia,           
                
                Lineas = new List<EditorLineaFactura> {
                            new EditorLineaFactura {
                                    Cantidad = 1,
                                    PorcentajeImpuesto = PorcentajeIvaPorDefecto
                            }
                      }
            };
        }
        public async Task<VisorFactura> BuscarVisorFacturaAsync(int? idFactura)
        {
            var factura = await BuscarFacturaAsync(idFactura); 
            var visor = new VisorFactura();
            visor.InyectarFactura(factura);
            return visor;
        }

        public async Task<EditorFactura> BuscaEditorFacturaAsync(int? idFactura)
        {
            var factura = await BuscarFacturaAsync(idFactura);
            var editor = new EditorFactura();
            editor.InyectarFactura(factura);
            return editor;
        }



        private async Task<Factura> ObtenerUlitmaFacturaDeLaSerie(string serie)
        {
            var consulta = _contexto.Facturas.AsQueryable();

            if (!string.IsNullOrEmpty(serie))
            {
                consulta = consulta.Where(m => m.SerieFactura == serie);
            }                    

            return await consulta
                         .OrderByDescending(m => m.FechaEmisionFactura)
                        .FirstOrDefaultAsync();
        }
         

        public async Task EnviarFacturaPorEmail(MensajeEmail mensaje, Factura factura)
        {
            await _servicioEmail.EnviarMensaje(mensaje);
            
            factura.EstadoFactura = EstadoFacturaEnum.Enviada;
            _contexto.SaveChanges();
        }

    }
}
