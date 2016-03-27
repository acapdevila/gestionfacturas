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
using System.IO;
using ClosedXML.Excel;
using PagedList.EntityFramework;
using PagedList;

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

        public async Task<IPagedList<LineaListaGestionFacturas>> ListaGestionFacturasAsync(FiltroBusquedaFactura filtroBusqueda)
        {
            var consulta = CrearConsultaFacturasFiltrada(filtroBusqueda);

            var consultaOrdenada = consulta.OrderBy_OrdenarPor(filtroBusqueda.OrdenarPorEnum);

            var consultaLineasFacturas = consultaOrdenada
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
                    Impuestos = m.Lineas.Sum(l => (decimal?)Math.Round((l.PrecioUnitario * l.Cantidad * l.PorcentajeImpuesto / 100),2)) ?? 0,
                    CompradorNombreOEmpresa = m.CompradorNombreOEmpresa,
                    ListaDescripciones = m.Lineas.Select(l => l.Descripcion),
                    CompradorNombreComercial = m.Comprador.NombreComercial
                });

            var facturas = await consultaLineasFacturas.ToPagedListAsync(filtroBusqueda.IndicePagina, filtroBusqueda.LineasPorPagina);

            return facturas;
        }

        public async Task<TotalesFacturas> ObtenerTotalesAsync(FiltroBusquedaFactura filtroBusqueda)
        {
           var consultaFacturas = CrearConsultaFacturasFiltrada(filtroBusqueda);

           var totales = await consultaFacturas.Select(m=> new
            {
                Id = 1,
                BaseImponible = m.Lineas.Sum(l => (decimal?)(l.PrecioUnitario * l.Cantidad)) ?? 0,
                Impuestos = m.Lineas.Sum(l => (decimal?)Math.Round((l.PrecioUnitario * l.Cantidad * l.PorcentajeImpuesto / 100),2)) ?? 0,
            })
            .GroupBy(m=> m.Id)
            .Select(g=> new TotalesFacturas {
                TotalBaseImponible = g.Sum(t=>t.BaseImponible),
                TotalImpuestos = g.Sum(t => t.Impuestos),
            }).FirstOrDefaultAsync();

            if (totales == null) totales = new TotalesFacturas();               
            
            return totales;
        }

        public async Task<EditorFactura> ObtenerEditorFacturaParaCrearNuevaFactura(string serie, int? idCliente)
        {
            EditorFactura editor;

            var ultimaFacturaCreada = await ObtenerUlitmaFacturaDeLaSerie(serie);

            if (ultimaFacturaCreada == null)
            {
                editor = new EditorFactura
                {
                    SerieFactura = serie,
                    NumeracionFactura = 1,
                    FormatoNumeroFactura = "{0}{1:1000}",
                    FechaEmisionFactura = DateTime.Today,
                    PorcentajeIvaPorDefecto = PorcentajeIvaPorDefecto,
                    FormaPago = FormaPagoEnum.Transferencia,
                    EstadoFactura = EstadoFacturaEnum.Creada,

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
                    FechaEmisionFactura = DateTime.Today,
                    NombreArchivoPlantillaInforme = ultimaFacturaCreada.NombreArchivoPlantillaInforme,
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

            if (idCliente.HasValue)
            {
                var cliente = _contexto.Clientes.Find(idCliente.Value);
                editor.AsignarDatosCliente(cliente); 
            }

            return editor;
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


        private IQueryable<Factura> CrearConsultaFacturasFiltrada(FiltroBusquedaFactura filtroBusqueda)
        {
            var consulta = _contexto.Facturas.AsQueryable();

            if (filtroBusqueda.TieneValores)
            {
                consulta = consulta.Where_FiltroBusqueda(filtroBusqueda);
            }
            
            return consulta;
        }

        private async Task<Factura> ObtenerUlitmaFacturaDeLaSerie(string serie)
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
         

        public void EnviarFacturaPorEmail(MensajeEmail mensaje, Factura factura)
        {
            _servicioEmail.EnviarMensaje(mensaje);
            
            factura.EstadoFactura = EstadoFacturaEnum.Enviada;
            _contexto.SaveChanges();
        }
        
        public async Task ImportarFacturasDeExcel(Stream stream, SelectorColumnasExcelFactura columnas, bool soloImportarFacturasDeClientesExistentes)
        {
            var editoresFacturas = ObtenerFacturasDeExcel(stream, columnas);

            var clientesExistentes = await _contexto.Clientes.ToListAsync();

            if (soloImportarFacturasDeClientesExistentes)
            {
                var idsClientes = clientesExistentes.Select(m => m.NumeroIdentificacionFiscal).ToList();
                editoresFacturas = editoresFacturas.Where(m => idsClientes.Contains(m.CompradorNumeroIdentificacionFiscal)).ToList();
            }

            CompletarDatosCompradores(editoresFacturas, clientesExistentes);
            
            await CrearFacturasAsync(editoresFacturas);
        }
             
        private List<EditorFactura> ObtenerFacturasDeExcel(Stream stream, SelectorColumnasExcelFactura columnas)
        {
            var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();

            var firstRow = worksheet.FirstRowUsed();
            var rowUsed = firstRow.RowUsed();
            rowUsed = rowUsed.RowBelow();

            var facturas = new List<EditorFactura>();

            while (!rowUsed.Cell(columnas.NumeroFactura).IsEmpty())
            {
                var factura = new EditorFactura();

                factura.IdUsuario = columnas.IdUsuario;

                factura.SerieFactura = columnas.SerieFactura.EsLetraMayuscula() ? rowUsed.Cell(columnas.SerieFactura).GetString() : columnas.SerieFactura;
                factura.NumeracionFactura = Convert.ToInt32(rowUsed.Cell(columnas.NumeroFactura).GetString());
                factura.FormatoNumeroFactura = columnas.FormatoNumeroFactura.EsLetraMayuscula() ? rowUsed.Cell(columnas.FormatoNumeroFactura).GetString() : columnas.FormatoNumeroFactura;
                factura.FechaEmisionFactura = rowUsed.Cell(columnas.FechaEmisionFactura).GetDateTime();
                factura.FechaVencimientoFactura = columnas.FechaVencimientoFactura.EsLetraMayuscula() ? rowUsed.Cell(columnas.FechaVencimientoFactura).GetDateTime() : string.IsNullOrEmpty(columnas.FechaVencimientoFactura) ? (DateTime?)null : Convert.ToDateTime(columnas.FechaVencimientoFactura);
                factura.EstadoFactura = (EstadoFacturaEnum)Enum.Parse(typeof(EstadoFacturaEnum), (columnas.EstadoFactura.EsLetraMayuscula() ? rowUsed.Cell(columnas.EstadoFactura).GetString() : columnas.EstadoFactura), true);

                factura.FormaPago = (FormaPagoEnum)Enum.Parse(typeof(FormaPagoEnum), (columnas.FormaPago.EsLetraMayuscula() ? rowUsed.Cell(columnas.FormaPago).GetString() : columnas.FormaPago), true);
                factura.FormaPagoDetalles = columnas.FormaPagoDetalles.EsLetraMayuscula() ? rowUsed.Cell(columnas.FormaPagoDetalles).GetString() : columnas.FormaPagoDetalles;

                factura.IdVendedor = columnas.IdVendedor.EsLetraMayuscula() ? Convert.ToInt32(rowUsed.Cell(columnas.IdVendedor).GetDouble()) : string.IsNullOrEmpty(columnas.IdVendedor) ? (int?)null : Convert.ToInt32(columnas.IdVendedor);
                factura.VendedorCodigoPostal = columnas.VendedorCodigoPostal.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorCodigoPostal).GetString() : columnas.VendedorCodigoPostal;
                factura.VendedorDireccion = columnas.VendedorDireccion.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorDireccion).GetString() : columnas.VendedorDireccion;
                factura.VendedorEmail = columnas.VendedorEmail.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorEmail).GetString() : columnas.VendedorEmail;
                factura.VendedorLocalidad = columnas.VendedorLocalidad.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorLocalidad).GetString() : columnas.VendedorLocalidad;
                factura.VendedorNombreOEmpresa = columnas.VendedorNombreOEmpresa.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorNombreOEmpresa).GetString() : columnas.VendedorNombreOEmpresa;
                factura.VendedorNumeroIdentificacionFiscal = columnas.VendedorNumeroIdentificacionFiscal.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorNumeroIdentificacionFiscal).GetString() : columnas.VendedorNumeroIdentificacionFiscal;
                factura.VendedorProvincia = columnas.VendedorProvincia.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorProvincia).GetString() : columnas.VendedorProvincia;

                factura.IdComprador = columnas.IdComprador.EsLetraMayuscula() ?  rowUsed.Cell(columnas.IdComprador).GetValue<int>() : string.IsNullOrEmpty(columnas.IdComprador) ? (int?)null : Convert.ToInt32(columnas.IdComprador);
                factura.CompradorCodigoPostal = columnas.CompradorCodigoPostal.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorCodigoPostal).GetString() : columnas.CompradorCodigoPostal;
                factura.CompradorDireccion1 = columnas.CompradorDireccion.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorDireccion).GetString() : columnas.CompradorDireccion;
                factura.CompradorEmail = columnas.CompradorEmail.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorEmail).GetString() : columnas.CompradorEmail;
                factura.CompradorLocalidad = columnas.CompradorLocalidad.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorLocalidad).GetString() : columnas.CompradorLocalidad;
                factura.CompradorNombreOEmpresa = columnas.CompradorNombreOEmpresa.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorNombreOEmpresa).GetString() : columnas.CompradorNombreOEmpresa;
                factura.CompradorNumeroIdentificacionFiscal = columnas.CompradorNumeroIdentificacionFiscal.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorNumeroIdentificacionFiscal).GetString() : columnas.CompradorNumeroIdentificacionFiscal;
                factura.CompradorProvincia = columnas.CompradorProvincia.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorProvincia).GetString() : columnas.CompradorProvincia;

                factura.PorcentajeIvaPorDefecto = columnas.PorcentajeImpuesto.EsLetraMayuscula() ? Convert.ToInt32(rowUsed.Cell(columnas.PorcentajeImpuesto).GetDouble()) : Convert.ToInt32(columnas.PorcentajeImpuesto);

                factura.Lineas = new List<EditorLineaFactura> {
                             new EditorLineaFactura {
                                     Cantidad = columnas.Cantidad.EsLetraMayuscula() ? Convert.ToInt32(rowUsed.Cell(columnas.Cantidad).GetDouble()) : Convert.ToInt32(columnas.Cantidad),
                                     PorcentajeImpuesto =  columnas.PorcentajeImpuesto.EsLetraMayuscula() ? Convert.ToInt32(rowUsed.Cell(columnas.PorcentajeImpuesto).GetDouble()) : Convert.ToInt32(columnas.PorcentajeImpuesto),
                                     Descripcion = rowUsed.Cell(columnas.Descripcion).GetString(),
                                     PrecioUnitario = Convert.ToDecimal(rowUsed.Cell(columnas.PrecioUnitario).GetDouble())
                             }
                    };

                factura.Comentarios = columnas.Comentarios.EsLetraMayuscula() ? rowUsed.Cell(columnas.Comentarios).GetString() : columnas.Comentarios;
                factura.ComentarioInterno = columnas.ComentarioInterno.EsLetraMayuscula() ? rowUsed.Cell(columnas.ComentarioInterno).GetString() : columnas.ComentarioInterno;
                factura.ComentariosPie = columnas.ComentariosPie.EsLetraMayuscula() ? rowUsed.Cell(columnas.ComentariosPie).GetString() : columnas.ComentariosPie;
                
                facturas.Add(factura);

                rowUsed = rowUsed.RowBelow();
            }

            return facturas.Distinct().ToList();
        }

        private void CompletarDatosCompradores(List<EditorFactura> editoresFacturas, List<Cliente> clientesExistentes)
        {
            foreach (var editor in editoresFacturas)
            {
                var clienteExistente = clientesExistentes.FirstOrDefault(m => m.NumeroIdentificacionFiscal == editor.CompradorNumeroIdentificacionFiscal);

                if (clienteExistente != null)
                {
                    if (!editor.IdComprador.HasValue)
                        editor.IdComprador = clienteExistente.Id;

                    if (string.IsNullOrEmpty(editor.CompradorNombreOEmpresa))
                        editor.CompradorNombreOEmpresa = clienteExistente.NombreOEmpresa;

                    if (string.IsNullOrEmpty(editor.CompradorDireccion1))
                        editor.CompradorDireccion1 = clienteExistente.Direccion1;

                    if (string.IsNullOrEmpty(editor.CompradorDireccion2))
                        editor.CompradorDireccion2 = clienteExistente.Direccion2;

                    if (string.IsNullOrEmpty(editor.CompradorLocalidad))
                        editor.CompradorLocalidad = clienteExistente.Localidad;
                                      
                    if (string.IsNullOrEmpty(editor.CompradorProvincia))
                        editor.CompradorProvincia = clienteExistente.Provincia;

                    if (string.IsNullOrEmpty(editor.CompradorCodigoPostal))
                        editor.CompradorCodigoPostal = clienteExistente.CodigoPostal;

                    if (string.IsNullOrEmpty(editor.CompradorEmail))
                        editor.CompradorEmail = clienteExistente.Email;
                }
            }
        }
    }
}
