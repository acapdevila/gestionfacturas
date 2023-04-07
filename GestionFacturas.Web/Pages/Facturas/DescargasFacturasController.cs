using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Extensions;
using Ionic.Zip;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class DescargasFacturasController : Controller
    {
        private readonly ServicioFactura _servicioFactura;
        private readonly IWebHostEnvironment _env;

        public DescargasFacturasController(ServicioFactura servicioFactura, IWebHostEnvironment env)
        {
            _servicioFactura = servicioFactura;
            _env = env;
        }

        public async Task<ActionResult> DescargarZip(FiltroBusquedaFactura filtroBusqueda)
        {
            if (!filtroBusqueda.TieneValores)
            {
                filtroBusqueda = new FiltroBusquedaFactura
                {
                    FechaDesde = ServicioFechas.PrimerDiaMesAnterior(),
                    FechaHasta = ServicioFechas.UltimoDiaMesActual()
                };
            }

            filtroBusqueda.LineasPorPagina = int.MaxValue;

            var listaGestionFacturas =
                await _servicioFactura.ListaGestionFacturasAsync(filtroBusqueda);

            if (!listaGestionFacturas.Any())
                return RedirectToPage("ListaGestionFacturas");

            var archivoZip = await GenerarZip(listaGestionFacturas);

            var nombreArchivoZip = string.Format("Facturas_desde_{0}_hasta_{1}.zip",
                filtroBusqueda.FechaDesde.Value.ToString("dd-MM-yyyy"),
                filtroBusqueda.FechaHasta.Value.ToString("dd-MM-yyyy"));

            archivoZip.Position = 0;
            HttpContext.Response.Headers.Add("content-disposition", "attachment; filename=" + nombreArchivoZip);
            return File(archivoZip, "application/zip");

        }

        private async Task<MemoryStream> GenerarZip(IEnumerable<LineaListaGestionFacturas> listaGestionFacturas)
        {
            var archivoZip = new MemoryStream();

            using (var zip = new ZipFile())
            {
                foreach (var itemFactura in listaGestionFacturas)
                {
                    var factura = await _servicioFactura.BuscarFacturaAsync(itemFactura.Id);
                    var informeLocal = GeneraLocalReportFactura.GenerarInformeLocalFactura(factura, _env.WebRootPath);

                    byte[] pdf = informeLocal.Render("PDF");
                    var nombrePdf = factura.Titulo().Replace(":", " ").Replace("·", "").Replace("€", "").Replace("/", "-").EliminarDiacriticos() + ".pdf";
                    zip.AddEntry(nombrePdf, pdf);
                }
                zip.Save(archivoZip);
            }

            return archivoZip;


        }

        public async Task<ActionResult> DescargarExcel(FiltroBusquedaFactura filtroBusqueda)
        {
            if (!filtroBusqueda.TieneValores)
            {
                filtroBusqueda = new FiltroBusquedaFactura
                {
                    FechaDesde = ServicioFechas.PrimerDiaMesAnterior(),
                    FechaHasta = ServicioFechas.UltimoDiaMesActual()
                };
            }

            filtroBusqueda.LineasPorPagina = int.MaxValue;

            var listaGestionFacturas =
                await _servicioFactura.ListaGestionFacturasAsync(filtroBusqueda);

            if (!listaGestionFacturas.Any())
                return RedirectToPage("ListaGestionFacturas");

            var workbook = ServicioExcel.GenerarExcelFactura(filtroBusqueda, listaGestionFacturas);

            var nombreArchivoExcel = string.Format("Facturacion_desde_{0}_hasta_{1}",
                filtroBusqueda.FechaDesde.Value.ToString("dd-MM-yyyy"),
                filtroBusqueda.FechaHasta.Value.ToString("dd-MM-yyyy"));

            return workbook.Deliver(nombreArchivoExcel);

            // or specify the content type:
            // return wb.Deliver("generatedFile.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
