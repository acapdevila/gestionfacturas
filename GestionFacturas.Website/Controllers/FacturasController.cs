﻿using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using GestionFacturas.Website.Viewmodels.Facturas;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.Reporting.WebForms;
using GestionFacturas.Website.Helpers;
using System.Linq;
using System.IO;
using Ionic.Zip;
using GestionFacturas.Website.Viewmodels.Email;
using GestionFacturas.Infra.Configuracion;

namespace GestionFacturas.Website.Controllers
{
    [Authorize]
    public class FacturasController : Controller
    {
        private readonly ServicioFactura _servicioFactura;

        public FacturasController(ServicioFactura servicioFactura)
        {
            _servicioFactura = servicioFactura;
        }


        public ActionResult Index()
        {
            return RedirectToAction("ListaGestionFacturas");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public async Task<ActionResult> ListaGestionFacturas(FiltroBusquedaFactura filtroBusqueda, int? pagina)
        {
            if (!filtroBusqueda.TieneValores)
            {
                filtroBusqueda = RecuperarFiltroBusqueda();

                if (pagina.HasValue)
                    filtroBusqueda.IndicePagina = pagina.Value;
            }

            var viewmodel = new ListaGestionFacturasViewModel {
                FiltroBusqueda = filtroBusqueda,
                ListaFacturas = await _servicioFactura.ListaGestionFacturasAsync(filtroBusqueda),
                Totales = await _servicioFactura.ObtenerTotalesAsync(filtroBusqueda)
            };

            GuardarFiltroBusqueda(filtroBusqueda);

            return View("ListaGestionFacturas", viewmodel);
        }
        
        public async Task<ActionResult> Detalles(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewmodel = new DetallesFacturaViewModel
            {
                Factura = await _servicioFactura.BuscarVisorFacturaAsync(id)
            };

            if (viewmodel.Factura == null) return HttpNotFound();

            return View(viewmodel);
        }

        public async Task<ActionResult> Crear(int? idCliente)
        {
            var viewmodel = new CrearFacturaViewModel {
                Factura = await _servicioFactura.ObtenerEditorFacturaParaCrearNuevaFactura(serie: string.Empty, idCliente: idCliente)
            };
            
            return View(viewmodel);
        }


        public async Task<ActionResult> Duplicar(int id)
        {
            var viewmodel = new CrearFacturaViewModel
            {
                Factura = await _servicioFactura.GenerarNuevoEditorFacturaDuplicado(id)
            };

            return View("Crear", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(CrearFacturaViewModel viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);

            viewmodel.Factura.IdUsuario = User.Identity.GetUserId();

            await _servicioFactura.CrearFacturaAsync(viewmodel.Factura);
            return RedirectToAction("Detalles", new { Id = viewmodel.Factura.Id });

        }

        public async Task<ActionResult> Editar(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewmodel = new EditarFacturaViewModel
            {
                Factura = await _servicioFactura.BuscaEditorFacturaAsync(id)
            };

            if (viewmodel.Factura == null) return HttpNotFound();
            
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(EditarFacturaViewModel viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);
               

            await _servicioFactura.ActualizarFacturaAsync(viewmodel.Factura);
            return RedirectToAction("Detalles", new { Id = viewmodel.Factura.Id });
        }

        [HttpPost]
        public async Task<ActionResult> CambiarEstado(EditorEstadoFactura editorEstadoFactura)
        {
            if (ModelState.IsValid)
            {
               await _servicioFactura.CambiarEstadoFacturaAsync(editorEstadoFactura);
            }

            return Json(new
            {
                editorEstadoFactura.IdFactura,
                editorEstadoFactura.NumeroFactura,
                editorEstadoFactura.EstadoFactura,
                TextoEstadoFactura = editorEstadoFactura.EstadoFactura.ObtenerNombreAtributoDisplay()
            });
        }

        public async Task<ActionResult> Eliminar(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewmodel = new EliminarFacturaViewModel {
                Factura = await _servicioFactura.BuscaEditorFacturaAsync(id.Value)
            };

            if (viewmodel.Factura == null) return HttpNotFound();

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Eliminar(EliminarFacturaViewModel viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);
            
            await _servicioFactura.EliminarFactura(viewmodel.Factura.Id);

            var numeroFacturaCodificada = WebUtility.UrlEncode(viewmodel.Factura.NumeroFactura);

            return RedirectToAction("EliminarConfirmado", new { numeroFacturaEliminada = numeroFacturaCodificada });
        }

        public ActionResult EliminarConfirmado(string numeroFacturaEliminada)
        {
            if (string.IsNullOrEmpty(numeroFacturaEliminada)) return HttpNotFound();

            ViewBag.NumeroFacturaEliminada = WebUtility.UrlDecode(numeroFacturaEliminada);

            return View("EliminarConfirmado");
        }

        public async Task<ActionResult> Imprimir(int id, string titulo)
        {
            var facturaAImprimir = await _servicioFactura.BuscarFacturaAsync(id);

            if (facturaAImprimir == null) return HttpNotFound();

            var informeLocal = GenerarInformeLocalFactura(facturaAImprimir);

            string mimeType;

            byte[] pdfBytes = ServicioPdf.GenerarPdfFactura(informeLocal, out mimeType);

            var cabecera = new System.Net.Mime.ContentDisposition
            {
                FileName = string.Format("{0}.pdf", titulo),

                // Si es verdadero el navegador trata de mostrar el archivo directamente
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cabecera.ToString());

            return File(pdfBytes, mimeType);
        }

        public async Task<ActionResult> Descargar(int id, string titulo)
        {
            var facturaAImprimir = await _servicioFactura.BuscarFacturaAsync(id);

            if (facturaAImprimir == null) return HttpNotFound();

            var informeLocal = GenerarInformeLocalFactura(facturaAImprimir);

            string mimeType;

            byte[] pdfBytes = ServicioPdf.GenerarPdfFactura(informeLocal, out mimeType);

            var cabecera = new System.Net.Mime.ContentDisposition
            {
                FileName = string.Format("{0}.pdf", titulo),

                // Si es verdadero el navegador trata de mostrar el archivo directamente
                Inline = false,
            };

            Response.AppendHeader("Content-Disposition", cabecera.ToString());

            return File(pdfBytes, mimeType);
        }


        public async Task<ActionResult> EnviarPorEmail(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var factura = await _servicioFactura.BuscaEditorFacturaAsync(id);

            if (factura == null) return HttpNotFound();

            var viewmodel = new EnviarFacturaPorEmailViewModel
            {
                IdFactura = factura.Id,
                NumeroFactura = factura.NumeroFactura,
                EditorEmail = new EditorEmail
                {
                    Remitente = factura.VendedorEmail,
                    Asunto = string.Format("{0} {1}", FactoriaConfiguracion.ObtenerConfiguracion().NombreComercialEmpresa , factura.NumeroFactura),
                    ContenidoHtml = @"Hola,",
                    Destinatario = factura.CompradorEmail              
                }
            };           
                
            return View(viewmodel);
        }

        public ActionResult Importar()
        {
            var viewmodel = new ImportarFacturasViewModel
            {
                SelectorColumnasExcel = new SelectorColumnasExcelFactura {
                                          
                },
                SoloImportarFacturasDeClientesExistentes = true
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Importar(ImportarFacturasViewModel viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);

            viewmodel.SelectorColumnasExcel.IdUsuario = User.Identity.GetUserId();

            await _servicioFactura.ImportarFacturasDeExcel(viewmodel.ArchivoExcelSeleccionado.InputStream, 
                                                           viewmodel.SelectorColumnasExcel, 
                                                           viewmodel.SoloImportarFacturasDeClientesExistentes);

            return RedirectToAction("ListaGestionFacturas");
        }


        [HttpPost]
        public async Task<ActionResult> EnviarPorEmail(EnviarFacturaPorEmailViewModel viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);

            var factura = await _servicioFactura.BuscarFacturaAsync(viewmodel.IdFactura);

            if (factura == null) return HttpNotFound();

            var mensaje = GenerarMensajeEmail(viewmodel.EditorEmail, factura);

            _servicioFactura.EnviarFacturaPorEmail(mensaje, factura);

            var numeroFacturaCodificada = WebUtility.UrlEncode(factura.NumeroFactura);

            return RedirectToAction("EnviarPorEmailConfirmado", new { numeroFacturaEnviada = numeroFacturaCodificada });

        }

        public ActionResult EnviarPorEmailConfirmado(string numeroFacturaEnviada)
        {
            if (string.IsNullOrEmpty(numeroFacturaEnviada)) return HttpNotFound();

            ViewBag.NumeroFacturaEnviada = WebUtility.UrlDecode(numeroFacturaEnviada);

            return View("EnviarPorEmailConfirmado");
        }

        public async Task<ActionResult> DescargarZip(FiltroBusquedaFactura filtroBusqueda)
        {
            if (!filtroBusqueda.TieneValores)
            {
                filtroBusqueda = FiltroBusquedaConValoresPorDefecto();
            }

            filtroBusqueda.LineasPorPagina = int.MaxValue;

            var listaGestionFacturas = await _servicioFactura.ListaGestionFacturasAsync(filtroBusqueda);
                        
            if (!listaGestionFacturas.Any())
                return RedirectToAction("ListaGestionFacturas");

            var archivoZip = await GenerarZip(listaGestionFacturas);

            var nombreArchivoZip = string.Format("Facturas_desde_{0}_hasta_{1}.zip", 
                filtroBusqueda.FechaDesde.Value.ToString("dd-MM-yyyy"),
                filtroBusqueda.FechaHasta.Value.ToString("dd-MM-yyyy"));

            archivoZip.Position = 0;
            HttpContext.Response.AppendHeader("content-disposition", "attachment; filename=" + nombreArchivoZip);
            return File(archivoZip, "application/zip");

        }

        public async Task<ActionResult> DescargarExcel(FiltroBusquedaFactura filtroBusqueda)
        {
            if (!filtroBusqueda.TieneValores)
            {
                filtroBusqueda = FiltroBusquedaConValoresPorDefecto();
            }

            filtroBusqueda.LineasPorPagina = int.MaxValue;

            var listaGestionFacturas = await _servicioFactura.ListaGestionFacturasAsync(filtroBusqueda);

            if (!listaGestionFacturas.Any())
                return RedirectToAction("ListaGestionFacturas");
            
            var workbook = ServicioExcel.GenerarExcelFactura(filtroBusqueda, listaGestionFacturas);

            var nombreArchivoExcel = string.Format("Facturacion_desde_{0}_hasta_{1}",
                 filtroBusqueda.FechaDesde.Value.ToString("dd-MM-yyyy"),
                 filtroBusqueda.FechaHasta.Value.ToString("dd-MM-yyyy"));

            return new ExcelResult(workbook, nombreArchivoExcel);
        }

        private FiltroBusquedaFactura FiltroBusquedaConValoresPorDefecto()
        {
            return new FiltroBusquedaFactura
            {
                FechaDesde = ServicioFechas.PrimerDiaMesAnterior(),
                FechaHasta = ServicioFechas.UltimoDiaMesActual()
            };
        }

        private FiltroBusquedaFactura RecuperarFiltroBusqueda()
        {
            var filtro = Session["FiltroBusquedaFacturas"];

            if (filtro != null)
                return (FiltroBusquedaFactura)filtro;

            return FiltroBusquedaConValoresPorDefecto();
        }

        private void GuardarFiltroBusqueda(FiltroBusquedaFactura filtro)
        {
            Session["FiltroBusquedaFacturas"] = filtro;
        }

        private string ObtenerRutaPlantillaInforme(Factura factura)
        {
            if (string.IsNullOrEmpty(factura.NombreArchivoPlantillaInforme))
                return Server.MapPath("~/Content/Informes/Factura.rdlc");

            return Server.MapPath("~/App_Data/Informes/" + factura.NombreArchivoPlantillaInforme);
        }

        private LocalReport GenerarInformeLocalFactura(Factura factura)
        {
            var rutaPlantillaInforme = ObtenerRutaPlantillaInforme(factura);

            var informeLocal = new LocalReport {
                ReportPath = rutaPlantillaInforme,
                EnableExternalImages = true                
            };

            var urlRaizWeb = RutaServidor.ObtenerUrlRaizWeb();

            var datasetFactura = factura.ConvertirADataSet(urlRaizWeb);

            informeLocal.DataSources.Add(new ReportDataSource("Facturas", datasetFactura.Tables[0]));
            informeLocal.DataSources.Add(new ReportDataSource("Lineas", datasetFactura.Tables[1]));            

            return informeLocal;
        }
        
        private MensajeEmail GenerarMensajeEmail(EditorEmail editorEmail, Factura factura)
        {
            var informeLocal = GenerarInformeLocalFactura(factura);

            string mimeType;

            byte[] facturaPdf = ServicioPdf.GenerarPdfFactura(informeLocal, out mimeType);

            var mensaje = new MensajeEmail
            {
                Asunto = editorEmail.Asunto,
                Cuerpo = editorEmail.ContenidoHtml,
                DireccionRemitente = editorEmail.Remitente,
                NombreRemitente = factura.VendedorNombreOEmpresa,
                DireccionesDestinatarios = new List<string> { editorEmail.Destinatario },
                Adjuntos = new List<ArchivoAdjunto> {
                                new ArchivoAdjunto
                                {
                                    Archivo = facturaPdf,
                                    MimeType = mimeType,
                                    Nombre = factura.Titulo + ".pdf"
                                }
                    }
            };

            return mensaje;
        }

        private async Task<MemoryStream> GenerarZip(IEnumerable<LineaListaGestionFacturas> listaGestionFacturas)
        {
            var archivoZip = new MemoryStream();

            using (var zip = new ZipFile())
            {
                foreach (var itemFactura in listaGestionFacturas)
                {
                    var factura = await _servicioFactura.BuscarFacturaAsync(itemFactura.Id);
                    var informeLocal = GenerarInformeLocalFactura(factura);
                    string mimeType;
                    byte[] renderedBytes = ServicioPdf.GenerarPdfFactura(informeLocal, out mimeType);
                    var nombrePdf = factura.Titulo.Replace(":", " ").Replace("·", "").Replace("€", "").Replace("/", "-").EliminarDiacriticos() + ".pdf";
                    zip.AddEntry(nombrePdf, renderedBytes);
                }
                zip.Save(archivoZip);
            }

            return archivoZip;
        }       



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _servicioFactura.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
