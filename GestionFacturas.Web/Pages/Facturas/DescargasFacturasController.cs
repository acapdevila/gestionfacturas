using GestionFacturas.Aplicacion;
using GestionFacturas.Dominio;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Extensions;
using GestionFacturas.AccesoDatosSql;
using Ionic.Zip;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.Web.Pages.Facturas
{
    public class DescargasFacturasController : Controller
    {
        private readonly SqlDb _db;
        private readonly IWebHostEnvironment _env;

        public DescargasFacturasController(IWebHostEnvironment env, SqlDb db)
        {
            _env = env;
            _db = db;
        }

        public async Task<ActionResult> DescargarZip(GridParamsFacturas gridParams)
        {
            var facturas =
                await _db.Facturas
                    .AsNoTracking()
                    .FiltrarPorParametros(gridParams)
                    .Select(m => new LineaListaGestionFacturas
                    {
                        Id = m.Id,
                        IdUsuario = m.IdUsuario,
                        IdComprador = m.IdComprador,
                        FormatoNumeroFactura = m.FormatoNumeroFactura,
                        NumeracionFactura = m.NumeracionFactura,
                        SerieFactura = m.SerieFactura,
                        FechaEmisionFacturaDateTime = m.FechaEmisionFactura,
                        FechaVencimientoFactura = m.FechaVencimientoFactura,
                        EstadoFactura = m.EstadoFactura,
                        BaseImponible = m.Lineas.Sum(l => (decimal?)(l.PrecioUnitario * l.Cantidad)) ?? 0,
                        Impuestos = m.Lineas.Sum(l => (decimal?)Math.Round((l.PrecioUnitario * l.Cantidad * l.PorcentajeImpuesto / 100), 2)) ?? 0,
                        CompradorNombreOEmpresa = m.CompradorNombreOEmpresa,
                        ListaDescripciones = m.Lineas.Select(l => l.Descripcion),
                        CompradorNombreComercial = m.Comprador.NombreComercial
                    })
                    .OrderBy_OrdenarPor(gridParams.Orden)
                    .ToListAsync();
            

            var archivoZip = await GenerarZip(facturas);

            var nombreArchivoZip = $"Facturas_desde_{gridParams.Desde}_hasta_{gridParams.Hasta}.zip";

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
                    var factura = await _db.Facturas
                        .Include(m => m.Lineas)
                        .FirstAsync(m => m.Id == itemFactura.Id);

                    var informeLocal = GeneraLocalReportFactura.GenerarInformeLocalFactura(factura, _env.WebRootPath);

                    byte[] pdf = informeLocal.Render("PDF");
                    var nombrePdf = factura.Titulo().Replace(":", " ").Replace("·", "").Replace("€", "").Replace("/", "-").EliminarDiacriticos() + ".pdf";
                    zip.AddEntry(nombrePdf, pdf);
                }
                zip.Save(archivoZip);
            }

            return archivoZip;


        }

        public async Task<ActionResult> DescargarExcel(GridParamsFacturas gridParams)
        {
            var facturas =
                await _db.Facturas
                .AsNoTracking()
                .FiltrarPorParametros(gridParams)
                .Select(m => new LineaListaGestionFacturas
                {
                    Id = m.Id,
                    IdUsuario = m.IdUsuario,
                    IdComprador = m.IdComprador,
                    FormatoNumeroFactura = m.FormatoNumeroFactura,
                    NumeracionFactura = m.NumeracionFactura,
                    SerieFactura = m.SerieFactura,
                    FechaEmisionFacturaDateTime = m.FechaEmisionFactura,
                    FechaVencimientoFactura = m.FechaVencimientoFactura,
                    EstadoFactura = m.EstadoFactura,
                    BaseImponible = m.Lineas.Sum(l => (decimal?)(l.PrecioUnitario * l.Cantidad)) ?? 0,
                    Impuestos = m.Lineas.Sum(l => (decimal?)Math.Round((l.PrecioUnitario * l.Cantidad * l.PorcentajeImpuesto / 100), 2)) ?? 0,
                    CompradorNombreOEmpresa = m.CompradorNombreOEmpresa,
                    ListaDescripciones = m.Lineas.Select(l => l.Descripcion),
                    CompradorNombreComercial = m.Comprador.NombreComercial
                })
                .OrderBy_OrdenarPor(gridParams.Orden)
                .ToListAsync();


            var workbook = ServicioExcel.GenerarExcelFactura(gridParams, facturas);

            var nombreArchivoExcel = $"Facturacion_desde_{gridParams.Desde}_hasta_{gridParams.Desde}.xlsx";

            return workbook.Deliver(nombreArchivoExcel,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            
        }
    }
}
