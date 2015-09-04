using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using GestionFacturas.Website.Viewmodels.Facturas;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System;
using Microsoft.Reporting.WebForms;
using GestionFacturas.Website.Helpers;

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
        public async Task<ActionResult> ListaGestionFacturas(FiltroBusquedaFactura filtroBusqueda)
        {
            if (!filtroBusqueda.TieneValores)
            {
                filtroBusqueda = new FiltroBusquedaFactura
                {
                    FechaDesde = ServicioFechas.PrimerDiaMesActual(),
                    FechaHasta = ServicioFechas.UltimoDiaMesActual()
                };
            }

            var viewmodel = new ListaGestionFacturasViewModel {
                    FiltroBusqueda = filtroBusqueda,
                    ListaFacturas = await _servicioFactura.ListaGestionFacturasAsync(filtroBusqueda)
            };            
       
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

        public async Task<ActionResult> Crear()
        {
            var viewmodel = new CrearFacturaViewModel {
                Factura = await _servicioFactura.ObtenerEditorFacturaParaCrearNuevaFactura(string.Empty)
            };
            return View(viewmodel);
        }

        // POST: Facturas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(EditarFacturaViewModel viewmodel)
        {
            if (!ModelState.IsValid) return View(viewmodel);
            
            await _servicioFactura.ActualizarFacturaAsync(viewmodel.Factura);
            return RedirectToAction("Detalles", new { Id = viewmodel.Factura.Id });
        }

        public async Task<ActionResult> Eliminar(int? id)
        {
            if (id == null)  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var viewmodel = new EliminarFacturaViewModel {
                Factura = await _servicioFactura.BuscaEditorFacturaAsync(id.Value)
            };
            
            if (viewmodel.Factura == null)  return HttpNotFound();
            
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

            byte[] renderedBytes = ServicioImpresion.GenerarPdf(informeLocal, out mimeType);

            var cabecera = new System.Net.Mime.ContentDisposition
            {
                FileName = string.Format("{0}.pdf", titulo),
                
                // Si es verdadero el navegador trata de mostrar el archivo directamente
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cabecera.ToString());
          
            return File(renderedBytes, mimeType);
        }

        public LocalReport GenerarInformeLocalFactura(Factura factura)
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

        private string ObtenerRutaPlantillaInforme(Factura factura)
        {
            if(string.IsNullOrEmpty(factura.NombreArchivoPlantillaInforme))
            return Server.MapPath("~/Content/Informes/Factura.rdlc");

            return Server.MapPath("~/Uploads/Informes/" + factura.NombreArchivoPlantillaInforme);
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
