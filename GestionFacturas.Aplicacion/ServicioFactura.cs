using ClosedXML.Excel;
using GestionFacturas.AccesoDatosSql;
using GestionFacturas.Dominio;
using GestionFacturas.Dominio.Clientes;
using GestionFacturas.Dominio.Infra;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace GestionFacturas.Aplicacion
{
    public class ServicioFactura : ServicioCrudFactura
    {
        private readonly ServicioEmail _servicioEmail;
        
        private int PorcentajeIvaPorDefecto
        {
            get { return 21; }
        }

        public ServicioFactura(
            SqlDb contexto, 
            ServicioEmail servicioEmail) : base(contexto)
        {
            _servicioEmail = servicioEmail;
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
                var factura = new EditorFactura
                {
                    IdUsuario = columnas.IdUsuario,
                    SerieFactura = columnas.SerieFactura.EsLetraMayuscula() ? rowUsed.Cell(columnas.SerieFactura).GetString() : columnas.SerieFactura,
                    NumeracionFactura = Convert.ToInt32(rowUsed.Cell(columnas.NumeroFactura).GetString()),
                    FormatoNumeroFactura = columnas.FormatoNumeroFactura.EsLetraMayuscula() ? rowUsed.Cell(columnas.FormatoNumeroFactura).GetString() : columnas.FormatoNumeroFactura,
                    FechaEmisionFactura = rowUsed.Cell(columnas.FechaEmisionFactura).GetString(),
                    FechaVencimientoFactura = columnas.FechaVencimientoFactura.EsLetraMayuscula() ? rowUsed.Cell(columnas.FechaVencimientoFactura).GetString() : columnas.FechaVencimientoFactura,
                    EstadoFactura = (EstadoFacturaEnum)Enum.Parse(typeof(EstadoFacturaEnum), (columnas.EstadoFactura.EsLetraMayuscula() ? rowUsed.Cell(columnas.EstadoFactura).GetString() : columnas.EstadoFactura), true),
                    FormaPago = (FormaPagoEnum)Enum.Parse(typeof(FormaPagoEnum), (columnas.FormaPago.EsLetraMayuscula() ? rowUsed.Cell(columnas.FormaPago).GetString() : columnas.FormaPago), true),
                    FormaPagoDetalles = columnas.FormaPagoDetalles.EsLetraMayuscula() ? rowUsed.Cell(columnas.FormaPagoDetalles).GetString() : columnas.FormaPagoDetalles,
                    IdVendedor = columnas.IdVendedor.EsLetraMayuscula() ? Convert.ToInt32(rowUsed.Cell(columnas.IdVendedor).GetDouble()) : string.IsNullOrEmpty(columnas.IdVendedor) ? (int?)null : Convert.ToInt32(columnas.IdVendedor),
                    VendedorCodigoPostal = columnas.VendedorCodigoPostal.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorCodigoPostal).GetString() : columnas.VendedorCodigoPostal,
                    VendedorDireccion = columnas.VendedorDireccion.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorDireccion).GetString() : columnas.VendedorDireccion,
                    VendedorEmail = columnas.VendedorEmail.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorEmail).GetString() : columnas.VendedorEmail,
                    VendedorLocalidad = columnas.VendedorLocalidad.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorLocalidad).GetString() : columnas.VendedorLocalidad,
                    VendedorNombreOEmpresa = columnas.VendedorNombreOEmpresa.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorNombreOEmpresa).GetString() : columnas.VendedorNombreOEmpresa,
                    VendedorNumeroIdentificacionFiscal = columnas.VendedorNumeroIdentificacionFiscal.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorNumeroIdentificacionFiscal).GetString() : columnas.VendedorNumeroIdentificacionFiscal,
                    VendedorProvincia = columnas.VendedorProvincia.EsLetraMayuscula() ? rowUsed.Cell(columnas.VendedorProvincia).GetString() : columnas.VendedorProvincia,
                    IdComprador = columnas.IdComprador.EsLetraMayuscula() ?  rowUsed.Cell(columnas.IdComprador).GetValue<int>() : string.IsNullOrEmpty(columnas.IdComprador) ? (int?)null : Convert.ToInt32(columnas.IdComprador),
                    CompradorCodigoPostal = columnas.CompradorCodigoPostal.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorCodigoPostal).GetString() : columnas.CompradorCodigoPostal,
                    CompradorDireccion1 = columnas.CompradorDireccion.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorDireccion).GetString() : columnas.CompradorDireccion,
                    CompradorEmail = columnas.CompradorEmail.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorEmail).GetString() : columnas.CompradorEmail,
                    CompradorLocalidad = columnas.CompradorLocalidad.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorLocalidad).GetString() : columnas.CompradorLocalidad,
                    CompradorNombreOEmpresa = columnas.CompradorNombreOEmpresa.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorNombreOEmpresa).GetString() : columnas.CompradorNombreOEmpresa,
                    CompradorNumeroIdentificacionFiscal = columnas.CompradorNumeroIdentificacionFiscal.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorNumeroIdentificacionFiscal).GetString() : columnas.CompradorNumeroIdentificacionFiscal,
                    CompradorProvincia = columnas.CompradorProvincia.EsLetraMayuscula() ? rowUsed.Cell(columnas.CompradorProvincia).GetString() : columnas.CompradorProvincia,
                    PorcentajeIvaPorDefecto = columnas.PorcentajeImpuesto.EsLetraMayuscula() ? Convert.ToInt32(rowUsed.Cell(columnas.PorcentajeImpuesto).GetDouble()) : Convert.ToInt32(columnas.PorcentajeImpuesto),
                    Lineas = new List<EditorLineaFactura> {
                        new EditorLineaFactura {
                            Cantidad = columnas.Cantidad.EsLetraMayuscula() ? Convert.ToInt32(rowUsed.Cell(columnas.Cantidad).GetDouble()) : Convert.ToInt32(columnas.Cantidad),
                            PorcentajeImpuesto =  columnas.PorcentajeImpuesto.EsLetraMayuscula() ? Convert.ToInt32(rowUsed.Cell(columnas.PorcentajeImpuesto).GetDouble()) : Convert.ToInt32(columnas.PorcentajeImpuesto),
                            Descripcion = rowUsed.Cell(columnas.Descripcion).GetString(),
                            PrecioUnitario = Convert.ToDecimal(rowUsed.Cell(columnas.PrecioUnitario).GetDouble())
                        }
                    },
                    Comentarios = columnas.Comentarios.EsLetraMayuscula() ? rowUsed.Cell(columnas.Comentarios).GetString() : columnas.Comentarios,
                    ComentarioInterno = columnas.ComentarioInterno.EsLetraMayuscula() ? rowUsed.Cell(columnas.ComentarioInterno).GetString() : columnas.ComentarioInterno,
                    ComentariosPie = columnas.ComentariosPie.EsLetraMayuscula() ? rowUsed.Cell(columnas.ComentariosPie).GetString() : columnas.ComentariosPie
                };

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
