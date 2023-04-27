using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Dominio;
using GestionFacturas.Dominio.Infra;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.Aplicacion
{
    public class ServicioFactura : ServicioCrudFactura
    {
        private int PorcentajeIvaPorDefecto
        {
            get { return 21; }
        }

        public ServicioFactura(
            SqlDb contexto) : base(contexto)
        {
        }

        public async Task<EditorFactura> ObtenerEditorFacturaParaCrearNuevaFactura(string serie, int? idCliente)
        {
            EditorFactura editor;

            var ultimaFacturaCreada = await ObtenerUlitmaFacturaDeLaSerie(serie);

            if (ultimaFacturaCreada is null)
            {
                editor = new EditorFactura
                {
                    SerieFactura = serie,
                    NumeracionFactura = 1,
                    FormatoNumeroFactura = "{0}{1:1000}",
                    FechaEmisionFactura = DateTime.Today.ToInputDate(),
                    PorcentajeIvaPorDefecto = PorcentajeIvaPorDefecto,
                    FormaPago = FormaPagoEnum.Transferencia,
                    EstadoFactura = EstadoFacturaEnum.Borrador,

                    Lineas = new List<EditorLineaFactura> {
                            new EditorLineaFactura {
                                    Cantidad = 1,
                                    PorcentajeImpuesto = PorcentajeIvaPorDefecto
                            }
                      }
                };
            }
            else
            {
                editor = new EditorFactura
                {
                    SerieFactura = ultimaFacturaCreada.SerieFactura,
                    NumeracionFactura = ultimaFacturaCreada.NumeracionFactura + 1,
                    FormatoNumeroFactura = ultimaFacturaCreada.FormatoNumeroFactura,
                    FechaEmisionFactura = DateTime.Today.ToInputDate(),
                    NombreArchivoPlantillaInforme = ultimaFacturaCreada.NombreArchivoPlantillaInforme,
                    PorcentajeIvaPorDefecto = PorcentajeIvaPorDefecto,
                    FormaPago = ultimaFacturaCreada.FormaPago,
                    FormaPagoDetalles = ultimaFacturaCreada.FormaPagoDetalles,
                    ComentariosPie = ultimaFacturaCreada.ComentariosPie,
                    EstadoFactura = EstadoFacturaEnum.Borrador,
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

            if (idCliente.HasValue)
            {
                var cliente = _contexto.Clientes.Find(idCliente.Value);
                editor.AsignarDatosCliente(cliente!); 
            }

            return editor;
        }


        public async Task<EditorFactura> GenerarNuevoEditorFacturaDuplicado(int idFactura)
        {
            var factura = await BuscarFacturaAsync(idFactura);

            var ultimaFacturaSerie = await ObtenerUlitmaFacturaDeLaSerie(factura.SerieFactura);

            var editor = new EditorFactura
            {
                SerieFactura = factura.SerieFactura,
                NumeracionFactura = ultimaFacturaSerie!.NumeracionFactura + 1,
                FormatoNumeroFactura = ultimaFacturaSerie.FormatoNumeroFactura,
                FechaEmisionFactura = DateTime.Today.ToInputDate(),
                NombreArchivoPlantillaInforme = factura.NombreArchivoPlantillaInforme,
                PorcentajeIvaPorDefecto = PorcentajeIvaPorDefecto,
                FormaPago = factura.FormaPago,
                FormaPagoDetalles = factura.FormaPagoDetalles,
                ComentariosPie = factura.ComentariosPie,
                EstadoFactura = EstadoFacturaEnum.Borrador,
                IdVendedor = factura.IdVendedor,
                VendedorCodigoPostal = factura.VendedorCodigoPostal,
                VendedorDireccion = factura.VendedorDireccion,
                VendedorEmail = factura.VendedorEmail,
                VendedorLocalidad = factura.VendedorLocalidad,
                VendedorNombreOEmpresa = factura.VendedorNombreOEmpresa,
                VendedorNumeroIdentificacionFiscal = factura.VendedorNumeroIdentificacionFiscal,
                VendedorProvincia = factura.VendedorProvincia,
                Lineas = new List<EditorLineaFactura>()
            };


            foreach (var linea in factura.Lineas)
            {
                editor.Lineas.Add(new EditorLineaFactura
                {
                    PorcentajeImpuesto = linea.PorcentajeImpuesto,
                    Cantidad = linea.Cantidad,
                    Descripcion     = linea.Descripcion,
                    PrecioUnitario = linea.PrecioUnitario
                });
            }
            var cliente = _contexto.Clientes.Find(factura.IdComprador);
            editor.AsignarDatosCliente(cliente!);

            return editor;
        }

        public async Task<VisorFactura> BuscarVisorFacturaAsync(int idFactura)
        {
            var factura = await BuscarFacturaAsync(idFactura); 
            var visor = new VisorFactura(factura);
            return visor;
        }

        public async Task<EditorFactura> BuscaEditorFacturaAsync(int idFactura)
        {
            var factura = await BuscarFacturaAsync(idFactura);
            var editor = new EditorFactura();
            editor.InyectarFactura(factura);
            editor.CompradorDireccion1 = factura.CompradorDireccion1();
            editor.CompradorDireccion2 = factura.CompradorDireccion2();
            editor.FechaEmisionFactura = factura.FechaEmisionFactura.ToInputDate();
            editor.FechaVencimientoFactura = factura.FechaVencimientoFactura?.ToInputDate();
            return editor;
        }

        
        private async Task<Factura?> ObtenerUlitmaFacturaDeLaSerie(string serie)
        {
            if (string.IsNullOrEmpty(serie))
            {
               var factura = await _contexto.Facturas.Where(m=>m.SerieFactura != null && m.SerieFactura != "")
                                .OrderByDescending(m=>m.FechaEmisionFactura)
                                .FirstOrDefaultAsync();

                if (factura == null) return null;

                serie = factura.SerieFactura;
            }

            var consulta = _contexto.Facturas.Where(m => m.SerieFactura == serie);

            return await consulta
                         .OrderByDescending(m => m.NumeracionFactura)
                        .FirstOrDefaultAsync();
        }
         
             
       
    }
}
